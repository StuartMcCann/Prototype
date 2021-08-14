using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prototype.Data;
using Prototype.Enums;
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

               
        [Key]
        public int CandidateID{get; set;}
        //remove level if enum works 
        

        public Boolean IsAvailable { get; set; } 

        public DateTime AvailableFrom { get; set;  }

        //skill will need one to many 
        public String Skill { get; set; }

        public double Rating { get; set; }
        public double Rate { get; set; }
        
        public Level LevelEnum { get; set;  }
        //normalised JobTitle
       
        public JobTitle JobTitleEnum { get; set; }
        
       

        //foreign Key with Users table
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
       

        //Forign Key for one to Many with Reviews
        public ICollection<Review> Reviews { get; set; }

        
        //foreign key one to many with Likes 
        public ICollection<Like> Likes { get; set; }
        //foreign key one to many with Contracts
        public ICollection<Contract> Contracts { get; set; }
        //many to many with Skills
        public virtual ICollection<Skill> Skills { get; set; }

       

        //default constructor 
        public Candidate()
        {
            this.Skills = new HashSet<Skill>(); 
           
        }

        //public Candidate(Candidate candidate, string userId, ApplicationDbContext db)
        //{
            

        //    this.UserId = userId;
        //    this.ApplicationUser = db.Users.Where(U => U.Id == userId).First();
        //    this.Skills = new HashSet<Skill>();
                        
        //}


    }

    //public enum Level
    //{
    //    [Display(Name = "Entry", Description = "1-2 years Experience" )]
    //    Entry,
    //    [Display(Name = "Intermediate", Description = "3-7 years Experience") ]
    //    Intermedidate,

    //    [Display(Name = "Expert", Description = "7+ years Experience")]
    //    Expert
    //}
}
