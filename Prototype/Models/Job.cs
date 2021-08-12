using Prototype.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prototype.Models
{
    public class Job
    {


        [Key]
        public int JobId { get; set; }

        public JobTitle JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public double UpperRate { get; set; }
        public double LowerRate { get; set; }

        public int Duration { get; set; }

        public String JobDescription { get; set; }
        public Boolean IsLive { get; set; }
        public Boolean IsFilled { get; set; }
        public Boolean IsUnderContract { get; set; }

        
        
        //normalised level 
       
        public Level Level { get; set; }

        //One to Many with Employer Table 
        public int EmployerRefId { get; set; }
        public Employer Employer { get; set; }

        /*Many to many With Candidates for Likes
        public ICollection<Candidate> Candidates { get; set; }*/
        public ICollection<Like> Likes { get; set; }

        // one to one with relationship with Contract
        public virtual Contract Contract { get; set; }

        //many to many relationship with skills
        public virtual ICollection<Skill> Skills { get; set; }

        public Job()
        {
            this.Skills = new HashSet<Skill>();
            this.IsFilled = false;
            this.IsLive = true;
            this.IsUnderContract = false; 
        }




        public void AddEmployer(Employer employer)
        {
            this.Employer = employer;
            this.EmployerRefId = employer.EmployerId;
        }


    }





}
