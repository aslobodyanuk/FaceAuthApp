using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartFaceAuth.Models
{
    public class FaceIdentificationResult
    {
        public Person Person { get; set; }

        public Group Group { get; set; }

        public bool Smile { get; set; }

        public bool Found { get; set; }

        public FaceIdentificationResult()
        {

        }

        public FaceIdentificationResult(bool found, Person person, Group group)
        {
            Found = found;
            Person = person;
            Group = group;
        }
    }
}