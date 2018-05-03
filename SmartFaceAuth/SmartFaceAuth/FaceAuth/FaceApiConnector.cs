using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.Web.Configuration;
using SmartFaceAuth.Models;

namespace SmartFaceAuth.FaceAuth
{
    public class FaceApiConnector
    {
        readonly IFaceServiceClient faceServiceClient = new FaceServiceClient(WebConfigurationManager.AppSettings["FaceApiSubKey"], WebConfigurationManager.AppSettings["FaceApiRoot"]);
        static double FaceConfidenceValue = Convert.ToDouble(WebConfigurationManager.AppSettings["FaceConfidenceValue"]);
        static double SmileConfidenceValue = Convert.ToDouble(WebConfigurationManager.AppSettings["SmileConfidenceValue"]);

        DbApiConnector apiConnector = new DbApiConnector();
        Logger logger = new Logger();

        public async Task DeleteUser(string personGroupId, Guid personId)
        {
            apiConnector.RemovePerson(Guid.Parse(personGroupId), personId);
            await faceServiceClient.DeletePersonFromPersonGroupAsync(personGroupId, personId);
        }

        public async Task<Group> AddGroup(string email)
        {
            Group newGroup = new Group(Guid.NewGuid(), email);
            newGroup = apiConnector.AddGroup(newGroup);
            await faceServiceClient.CreatePersonGroupAsync(newGroup.GroupId.ToString(), newGroup.Email);
            return newGroup;
        }
        
        public async Task UpdatePerson(string personGroupId, string personGuid, string imagePath)
        {
            Microsoft.ProjectOxford.Face.Contract.Person updPerson = await faceServiceClient.GetPersonInPersonGroupAsync(personGroupId, Guid.Parse(personGuid));
            apiConnector.SetTrained(false, Guid.Parse(personGroupId));
            using (Stream stream = File.OpenRead(imagePath))
            {
                await faceServiceClient.AddPersonFaceInPersonGroupAsync(personGroupId, updPerson.PersonId, stream);
            }
        }

        public async Task AddToGroup(string personGroupId, string personName, string imagePath)
        {
            CreatePersonResult newPerson = await faceServiceClient.CreatePersonInPersonGroupAsync(personGroupId, personName);
            Models.Person personToDB = new Models.Person(personName, newPerson.PersonId, new ImageData(ImageToByteArray(imagePath)));
            apiConnector.AddPerson(Guid.Parse(personGroupId), personToDB);

            using (Stream s = File.OpenRead(imagePath))
            {
                await faceServiceClient.AddPersonFaceInPersonGroupAsync(personGroupId, newPerson.PersonId, s);
            }
        }

        public async Task<Status> TrainGroup(string personGroupId)
        {
            await faceServiceClient.TrainPersonGroupAsync(personGroupId);
            Status trainingResult = await WaitForTraining(personGroupId);
            apiConnector.SetTrained(true, Guid.Parse(personGroupId));
            return trainingResult;
        }

        public async Task<Status> WaitForTraining(string personGroupId)
        {
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceServiceClient.GetPersonGroupTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != Status.Running)
                {
                    break;
                }

                await Task.Delay(500);
            }

            return trainingStatus.Status;
        }

        public async Task<FaceIdentificationResult> IdentifyPerson(string email, string imagePath)
        {
            Group group = apiConnector.GetGroup(email);
            if (group != null)
            {
                //Status st = await TrainGroup(group.GroupId.ToString());
                using (Stream stream = File.OpenRead(imagePath))
                {
                    var requiredFaceAttributes = new FaceAttributeType[] { FaceAttributeType.Smile };
                    var faces = await faceServiceClient.DetectAsync(stream, returnFaceAttributes: requiredFaceAttributes);
                    if (faces.Length > 0)
                    {
                        var faceIds = faces.Select(face => face.FaceId).ToArray();
                        var results = await faceServiceClient.IdentifyAsync(faceIds, group.GroupId.ToString());

                        foreach (var identifyResult in results)
                        {
                            if (identifyResult.Candidates.Length > 0)
                            {
                                if (identifyResult.Candidates[0].Confidence > FaceConfidenceValue)
                                {
                                    var candidateId = identifyResult.Candidates[0].PersonId;
                                    var person = await faceServiceClient.GetPersonInPersonGroupAsync(group.GroupId.ToString(), candidateId);
                                    Models.Person personOutput = new Models.Person(person.Name, person.PersonId);
                                    FaceIdentificationResult faceIdResult = new FaceIdentificationResult(true, personOutput, group);
                                    Face personFace = faces.SingleOrDefault(x => x.FaceId.Equals(identifyResult.FaceId));
                                    if (personFace.FaceAttributes.Smile > SmileConfidenceValue)
                                    {
                                        faceIdResult.Smile = true;
                                    }
                                    else
                                    {
                                        faceIdResult.Smile = false;
                                    }
                                    return faceIdResult;
                                }
                            }
                        }
                    }
                }
            }
            Log authLog = new Log(group.GroupId, DateTime.Now, "Login attempt.", new ImageData(ImageToByteArray(imagePath)));
            logger.WriteToDatabase(authLog);
            FaceIdentificationResult badResult = new FaceIdentificationResult();
            badResult.Found = false;
            return badResult;
        }

        public byte[] ImageToByteArray(string filePath)
        {
            byte[] imgData = File.ReadAllBytes(filePath);
            return imgData;
        }
    }
}