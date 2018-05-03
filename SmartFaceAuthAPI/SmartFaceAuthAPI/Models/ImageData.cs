using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SmartFaceAuthAPI.Models
{
    public class ImageData
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public ImageData()
        {

        }

        public ImageData(byte[] content)
        {
            Content = content;
        }
    }
}