using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Candidate : User
    {
        [Key]
        public int CandidateId { get; set; }
        public String Name { get; set; }
        public String Level { get; set; }

        public String Skill { get; set; }

        public double Rating { get; set; }
        public double Rate { get; set; }


        //Forign Key for one to Many with Reviews
        public ICollection<Review> Reviews { get; set; }
    }
}
