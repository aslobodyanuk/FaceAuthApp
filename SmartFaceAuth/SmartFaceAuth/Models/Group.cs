using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFaceAuth.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public Guid GroupId { get; set; }

        public bool IsTrained { get; set; }

        public virtual List<Person> PersonsList { get; set; }

        public Group()
        {

        }

        public Group(Guid groupGuid)
        {
            GroupId = groupGuid;
        }

        public Group(Guid groupGuid, string email)
        {
            GroupId = groupGuid;
            Email = email;
        }
    }
}