using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class CandidateProfile: Candidate
    {
              
       
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public Byte[] ProfilePicture { get; set; }
    }
}
