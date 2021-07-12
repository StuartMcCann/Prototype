using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Candidate 
    {

        /* public Candidate()
         {
             this.Jobs = new HashSet<Job>();
             this.Employers = new HashSet<Employer>(); 
         }*/

        
        [Key]
        public int CandidateID{get; set;}
        public String Name { get; set; }
        //remove level if enum works 
        public String Level { get; set; }

        //skill will need one to many 
        public String Skill { get; set; }

        public double Rating { get; set; }
        public double Rate { get; set; }

        public Level LevelEnum { get; set;  }
        [Display(Name = "Profile Picture")]
        public byte[] ProfilePicture { get; set; }

        //foreign Key with Users table
        public virtual ApplicationUser ApplicationUser { get; set; }

        //Forign Key for one to Many with Reviews
        public ICollection<Review> Reviews { get; set; }

        /*Many to many With Jobs and Employer for Likes
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Employer> Employers { get; set; }*/
        public ICollection<Like> Likes { get; set; }
    }

    public enum Level
    {
        [Display(Name = "Entry", Description = "1-2 years Experience" )]
        Entry,
        [Display(Name = "Intermediate", Description = "3-7 years Experience") ]
        Intermedidate,

        [Display(Name = "Expert", Description = "7+ years Experience")]
        Expert
    }
}
