using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFaceAuthAPI.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Guid PersonId { get; set; }

        public ImageData PersonImage { get; set; }

        public Person()
        {

        }

        public Person(string name, Guid guid)
        {
            Name = name;
            PersonId = guid;
        }

        public Person(string name, Guid guid, ImageData imageData)
        {
            Name = name;
            PersonId = guid;
            PersonImage = imageData;
        }
    }
}