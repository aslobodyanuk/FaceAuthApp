using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFaceAuth.Models
{
    public class FaceData
    {
        public string Name { get; set; }

        public Guid PersonId { get; set; }

        public Guid GroupId { get; set; }
    }
}