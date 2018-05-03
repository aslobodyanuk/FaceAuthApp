using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Media.Imaging;
using SmartFaceAuth.Models;

namespace SmartFaceAuth.FaceAuth
{
    internal static class UIHelper
    {
        public static IEnumerable<vmFace> CalculateFaceRectangleForRendering(IEnumerable<Microsoft.ProjectOxford.Face.Contract.Face> faces, int maxSize, Tuple<int, int> imageInfo)
        {
            var imageWidth = imageInfo.Item1;
            var imageHeight = imageInfo.Item2;
            float ratio = (float)imageWidth / imageHeight;

            int uiWidth = 0;
            int uiHeight = 0;

            uiWidth = maxSize;
            uiHeight = (int)(maxSize / ratio);

            float scale = (float)uiWidth / imageWidth;

            foreach (var face in faces)
            {
                yield return new vmFace()
                {
                    FaceId = face.FaceId.ToString(),
                    Left = (int)(face.FaceRectangle.Left * scale),
                    Top = (int)(face.FaceRectangle.Top * scale),
                    Height = (int)(face.FaceRectangle.Height * scale),
                    Width = (int)(face.FaceRectangle.Width * scale),
                };
            }
        }

        public static Tuple<int, int> GetImageInfoForRendering(string imageFilePath)
        {
            try
            {
                using (var s = File.OpenRead(imageFilePath))
                {
                    BitmapDecoder decode = BitmapDecoder.Create(s, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
                    var frame1 = decode.Frames.First();
                    // Store image width and height for following rendering
                    return new Tuple<int, int>(frame1.PixelWidth, frame1.PixelHeight);
                }
            }
            catch (Exception e)
            {
                new Logger().WriteToFile(e);
                return new Tuple<int, int>(0, 0);
            }
        }
    }
}