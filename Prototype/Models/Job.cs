using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Job
    {

        /*public Job()
        {
            this.Candidates = new HashSet<Candidate>(); 
        }*/

        [Key]
        public int JobId { get; set; }

        public String JobTitle { get; set; }
        public  DateTime StartDate { get; set; }
        public  double UpperRate { get; set; }
        public double LowerRate { get; set; }

        public int Duration { get; set;  }

        public String JobDescription { get; set; }
        public Boolean IsLive { get; set; }
        public Boolean IsFilled { get; set; }
        public Boolean IsUnderContract { get; set; }

        //Foreign Keys
        //Normalised Job Title
        public int JobTitleRefId { get; set; }
        public JobTitle JobTitleFK { get; set; }

        //One to Many with Employer Table 
        public int EmployerRefId { get; set; }
        public Employer Employer { get; set; }

        /*Many to many With Candidates for Likes
        public ICollection<Candidate> Candidates { get; set; }*/
        public ICollection<Like> Likes { get; set; }
        

    }

    

}
