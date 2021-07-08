using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Candidate : User
    {

       /* public Candidate()
        {
            this.Jobs = new HashSet<Job>();
            this.Employers = new HashSet<Employer>(); 
        }*/

        [Key]
        public int CandidateId { get; set; }
        public String Name { get; set; }
        public String Level { get; set; }

        public String Skill { get; set; }

        public double Rating { get; set; }
        public double Rate { get; set; }


        //Forign Key for one to Many with Reviews
        public ICollection<Review> Reviews { get; set; }

        /*Many to many With Jobs and Employer for Likes
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Employer> Employers { get; set; }*/
        public ICollection<Like> Likes { get; set; }
    }
}
