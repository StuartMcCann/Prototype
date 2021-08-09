using Prototype.Data;
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
        //remove level if enum works 
        public String Level { get; set; }

        public Boolean IsAvailable { get; set; } 

        public DateTime AvailableFrom { get; set;  }

        //skill will need one to many 
        public String Skill { get; set; }

        public double Rating { get; set; }
        public double Rate { get; set; }

        public Level LevelEnum { get; set;  }
        

        //foreign Key with Users table
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }

        //Forign Key for one to Many with Reviews
        public ICollection<Review> Reviews { get; set; }

        /*Many to many With Jobs and Employer for Likes
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Employer> Employers { get; set; }*/
        //foreign key one to many with Likes 
        public ICollection<Like> Likes { get; set; }
        //foreign key one to many with Contracts
        public ICollection<Contract> Contracts { get; set; }

        //default constructor 
        public Candidate()
        {

        }

        public Candidate(Candidate candidate, string userId, ApplicationDbContext db)
        {
            this.UserId = userId;
            this.ApplicationUser = db.Users.Where(U => U.Id == userId).First(); 
        }


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
