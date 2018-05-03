using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFaceAuthAPI.Models
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
            PersonsList = new List<Person>();
        }
    }
}