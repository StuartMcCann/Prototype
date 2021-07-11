using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Employer:User 
    {

       /* public Employer()
        {
            this.Candidates = new HashSet<Candidate>();
        }*/
        [Key]
        public int EmployerId { get; set; }
        public String CompanyName { get; set; }
        public double Rating { get; set; }

        //Forign Key for one to Many with Jobs
        public ICollection<Job> Jobs { get; set;  }

        /*Many to many With Candidates for Likes
        public ICollection<Candidate> Candidates { get; set; }*/
        public ICollection<Like> Likes { get; set; }

    }
}
