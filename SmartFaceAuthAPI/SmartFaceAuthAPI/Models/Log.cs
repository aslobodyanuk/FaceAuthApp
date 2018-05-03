using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SmartFaceAuthAPI.Models
{
    public class Log
    {
        public int Id { get; set; }

        public Guid UserGuid { get; set; }

        public DateTime? Time { get; set; }

        public string Message { get; set; }

        public virtual ImageData AuthImage { get; set; }

        public Log()
        {

        }

        public Log(Guid userGuid, DateTime time, string message)
        {
            UserGuid = userGuid;
            Time = time;
            Message = message;
        }

        public Log(Guid userGuid, DateTime time, string message, ImageData authImage)
        {
            UserGuid = userGuid;
            Time = time;
            Message = message;
            AuthImage = authImage;
        }
    }
}