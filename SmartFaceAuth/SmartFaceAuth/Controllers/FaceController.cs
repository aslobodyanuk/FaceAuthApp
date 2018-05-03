using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SmartFaceAuth.Models;
using SmartFaceAuth.FaceAuth;
using System.Collections.Concurrent;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace SmartFaceAuth.Controllers
{
    [Authorize]
    [RoutePrefix("Face")]
    public class FaceController : Controller
    {
        static string ServiceKey = ConfigurationManager.AppSettings["FaceApiSubKey"];
        static string ApiRoot = ConfigurationManager.AppSettings["FaceApiRoot"];
        static string Directory = "../UploadedFiles";
        static int MaxImageSize = Convert.ToInt32(ConfigurationManager.AppSettings["MaxImageSize"]);

        FaceApiConnector apiConnector = new FaceApiConnector();
        DbApiConnector dbConnector = new DbApiConnector();
        Logger logger = new Logger();

        public async Task<ActionResult> Index()
        {
            return await FaceSettings();
        }

        [HttpGet]
        public ActionResult AddNewUserFace()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> AuthLogs()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Group group = dbConnector.GetGroup(user.Email);
            if (group != null)
            {
                Log[] logList = dbConnector.GetLogsList(group.GroupId);
                AuthLogs authLogs = new AuthLogs();
                List<string> dates = new List<string>();
                List<string> imgData = new List<string>();
                foreach (Log value in logList)
                {
                    dates.Add(value.Time.Value.ToString());
                    imgData.Add(String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(value.AuthImage.Content)));
                }
                authLogs.Dates = dates.ToArray();
                authLogs.SrcImages = imgData.ToArray();
                return View(authLogs);
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }

        [HttpGet]
        public async Task ExecuteGroupTraining()
        {
            PersonForJs groupData = (PersonForJs)Session["groupData"];
            await apiConnector.TrainGroup(groupData.GroupId.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> DeleteUser(int id)
        {
            PersonForJs groupData = (PersonForJs)Session["groupData"];
            await apiConnector.DeleteUser(groupData.GroupId.ToString(), groupData.PersonIds[id]);
            return Redirect("/Face/FaceSettings");
        }

        [HttpGet]
        public async Task<ActionResult> AddUserFace(int id)
        {
            PersonForJs groupData = (PersonForJs)Session["groupData"];
            FaceData faceData = new FaceData();
            faceData.Name = groupData.Names[id];
            faceData.PersonId = groupData.PersonIds[id];
            faceData.GroupId = groupData.GroupId;
            Session["curFaceData"] = faceData;
            return View(faceData);
        }

        [HttpGet]
        public async Task<ActionResult> FaceSettings()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                            .GetUserManager<ApplicationUserManager>()
                            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            Group group = dbConnector.GetGroup(user.Email);
            PersonForJs groupData;
            if (group != null)
            {
                List<string> names = group.PersonsList.Select(x => x.Name).ToList();
                List<Guid> personGuids = group.PersonsList.Select(x => x.PersonId).ToList();
                List<string> baseImages = new List<string>();
                foreach (byte[] value in group.PersonsList.Select(x => x.PersonImage.Content))
                {
                    var base64 = Convert.ToBase64String(value);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    baseImages.Add(imgSrc);
                }
                List<int> indexes = new List<int>();
                for (int counter = 0; counter < names.Count; counter++)
                {
                    indexes.Add(counter);
                }

                groupData = new PersonForJs(indexes, names, personGuids, baseImages, group.GroupId, group.IsTrained);
                Session["groupData"] = groupData;
            }
            else
            {
                group = await apiConnector.AddGroup(user.Email);
                groupData = new PersonForJs();
                groupData.GroupId = group.GroupId;
                groupData.IsTrained = true;
                Session["groupData"] = groupData;
            }

            return View(groupData);
        }

        [HttpGet]
        public async Task ChosePersonToAdd(Guid id)
        {
            PersonForJs groupData = (PersonForJs)Session["groupData"];
            Dictionary<Guid, string> croppedImagesLocations = (Dictionary<Guid, string>)Session["croppedImagesLocations"];
            FaceData faceData = (FaceData)Session["curFaceData"];
            await apiConnector.UpdatePerson(groupData.GroupId.ToString(), faceData.PersonId.ToString(), croppedImagesLocations[id]);
        }

        [HttpGet]
        public async Task ChoseNewPersonToAdd(Guid id, string name)
        {
            PersonForJs groupData = (PersonForJs)Session["groupData"];
            Dictionary<Guid, string> croppedImagesLocations = (Dictionary<Guid, string>)Session["croppedImagesLocations"];
            await apiConnector.AddToGroup(groupData.GroupId.ToString(), name, croppedImagesLocations[id]);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> ValidatePicture(string email)
        {
            string uplImageName = (string)Session["uplImageName"];
            var fullImgPath = Server.MapPath(Directory) + "\\" + uplImageName as string;
            Group curGroup = dbConnector.GetGroup(email);
            bool emailValid = curGroup != null;
            string name = "DefaultName";
            bool isValid = false;
            bool smile = false;
            bool isTrainedGroup = false;
            if (emailValid)
            {
                isTrainedGroup = dbConnector.GetIsTrained(curGroup.GroupId);
                if (isTrainedGroup)
                {
                    FaceIdentificationResult result = await apiConnector.IdentifyPerson(email, fullImgPath);
                    Session["faceIdResult"] = result;
                    if (result.Person != null)
                    {
                        name = result.Person.Name;
                    }
                    isValid = result.Found;
                    smile = result.Smile;
                }
            }

            return new JsonResult
            {
                Data = new
                {
                    IsValid = isValid,
                    Name = name,
                    Smile = smile,
                    IsTrained = isTrainedGroup,
                    EmailValid = emailValid
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> SaveCandidateFiles()
        {
            string fileName = string.Empty;
            
            HttpFileCollection fileRequested = System.Web.HttpContext.Current.Request.Files;
            if (fileRequested != null)
            {
                CreateDirectory();

                for (int i = 0; i < fileRequested.Count; i++)
                {
                    var file = Request.Files[i];
                    fileName = Guid.NewGuid() + ".jpg";

                    try
                    {
                        file.SaveAs(Path.Combine(Server.MapPath(Directory), fileName));
                        Session["uplImageName"] = fileName;
                    }
                    catch (Exception e)
                    {
                        logger.WriteToFile(e);
                    }
                }
            }
            return new JsonResult();
        }

        [HttpGet]
        public async Task<dynamic> GetDetectedFaces()
        {
            ObservableCollection<vmFace> detectedFaces = new ObservableCollection<vmFace>();
            ObservableCollection<vmFace> resultCollection = new ObservableCollection<vmFace>();

            Dictionary<Guid, string> croppedImagesLocations = new Dictionary<Guid, string>();
            string uplImageName = (string)Session["uplImageName"];
            var fullImgPath = Server.MapPath(Directory) + '/' + uplImageName as string;
            var queryFaceImageUrl = Directory + '/' + uplImageName;

            if (uplImageName != "")
            {
                CreateDirectory();

                try
                {
                    using (var fStream = System.IO.File.OpenRead(fullImgPath))
                    {
                        var imageInfo = UIHelper.GetImageInfoForRendering(fullImgPath);
                        var faceServiceClient = new FaceServiceClient(ServiceKey, ApiRoot);
                        Face[] faces = await faceServiceClient.DetectAsync(fStream, true, true, new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Glasses });
                        Bitmap CroppedFace = null;

                        foreach (var face in faces)
                        {
                            var croppedImg = Convert.ToString(Guid.NewGuid()) + ".jpeg" as string;
                            var croppedImgPath = Directory + '/' + croppedImg as string;
                            var croppedImgFullPath = Server.MapPath(Directory) + '/' + croppedImg as string;
                            CroppedFace = CropBitmap(
                                            (Bitmap)Image.FromFile(fullImgPath),
                                            face.FaceRectangle.Left,
                                            face.FaceRectangle.Top,
                                            face.FaceRectangle.Width,
                                            face.FaceRectangle.Height);
                            CroppedFace.Save(croppedImgFullPath, ImageFormat.Jpeg);
                            if (CroppedFace != null)
                                ((IDisposable)CroppedFace).Dispose();
                            croppedImagesLocations.Add(face.FaceId, croppedImgFullPath);

                            detectedFaces.Add(new vmFace()
                            {
                                ImagePath = fullImgPath,
                                FileName = croppedImg,
                                FilePath = croppedImgPath,
                                Left = face.FaceRectangle.Left,
                                Top = face.FaceRectangle.Top,
                                Width = face.FaceRectangle.Width,
                                Height = face.FaceRectangle.Height,
                                FaceId = face.FaceId.ToString(),
                                Gender = face.FaceAttributes.Gender,
                                Age = string.Format("{0:#} years old", face.FaceAttributes.Age),
                                IsSmiling = face.FaceAttributes.Smile > 0.0 ? "Smile" : "Not Smile",
                                Glasses = face.FaceAttributes.Glasses.ToString(),
                            });
                        }
                        Session["croppedImagesLocations"] = croppedImagesLocations;
                        
                        var rectFaces = UIHelper.CalculateFaceRectangleForRendering(faces, MaxImageSize, imageInfo);
                        foreach (var face in rectFaces)
                        {
                            resultCollection.Add(face);
                        }
                    }
                }
                catch (FaceAPIException e)
                {
                    logger.WriteToFile(e);
                }
            }
            return new JsonResult
            {
                Data = new
                {
                    QueryFaceImage = queryFaceImageUrl,
                    MaxImageSize = MaxImageSize,
                    FaceInfo = detectedFaces,
                    FaceRectangles = resultCollection
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight)
        {
            Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }

        private void CreateDirectory()
        {
            bool exists = System.IO.Directory.Exists(Server.MapPath(Directory));
            if (!exists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(Directory));
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }
    }
}