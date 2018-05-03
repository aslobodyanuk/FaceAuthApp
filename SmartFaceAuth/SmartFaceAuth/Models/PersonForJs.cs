using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFaceAuth.Models
{
    public class PersonForJs
    {
        public bool IsTrained { get; set; }

        public Guid GroupId { get; set; }

        public List<int> Ids { get; set; }

        public List<string> Names { get; set; }

        public List<Guid> PersonIds { get; set; }

        public List<string> SrcImages { get; set; }

        public PersonForJs()
        {
            Ids = new List<int>();
            Names = new List<string>();
            PersonIds = new List<Guid>();
            SrcImages = new List<string>();
        }

        public PersonForJs(List<int> ids, List<string> names, List<Guid> persId, List<string> srcImage, Guid groupId)
        {
            Ids = ids;
            Names = names;
            PersonIds = persId;
            GroupId = groupId;
            SrcImages = srcImage;
        }

        public PersonForJs(List<int> ids, List<string> names, List<Guid> persId, List<string> srcImage, Guid groupId, bool isTrained)
        {
            Ids = ids;
            Names = names;
            PersonIds = persId;
            GroupId = groupId;
            SrcImages = srcImage;
            IsTrained = isTrained;
        }
    }
}