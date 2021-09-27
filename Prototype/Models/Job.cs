using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Models
{
    public class Job
    {
        private const double MinimumWage = 8.21;

        [Key]
        public int JobId { get; set; }
        [Required]
        [Display(Name =  "Job Title")]
        public JobTitle JobTitleEnum { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Range(MinimumWage, double.MaxValue, ErrorMessage = "Please select a value over national living wage")]
        [Display(Name = "Lower Rate")]
        public double LowerRate { get; set; }
        [Required]
        [Display(Name = "Upper Rate")]
        public double UpperRate { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Contracts must be at least 1 week in duration")]
        public int Duration { get; set; }
        [Required]
        [StringLength(maximumLength:1000, MinimumLength =100, ErrorMessage ="JobDescription must be between 100 - 1000 characters")]
        [Display(Name = "Job Description")]
        public String JobDescription { get; set; }
        public Boolean IsLive { get; set; }
        public Boolean IsFilled { get; set; }
        public Boolean IsUnderContract { get; set; }


        [Required]
        public Level LevelEnum { get; set; }

        //One to Many with Employer Table 
        public int EmployerRefId { get; set; }
        public Employer Employer { get; set; }
        //below represents relationship with likes      
        public ICollection<Like> Likes { get; set; }

        // one to one with relationship with Contract
        public virtual Contract Contract { get; set; }


        //many to many relationship with skills
        
        public virtual ICollection<Skill> Skills { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Please select at least 3 skills")]
        [NotMapped]
        public IEnumerable<int> SkillIds { get; set; }


        public Job()
        {
            this.Skills = new HashSet<Skill>();
            this.IsFilled = false;
            this.IsLive = true;
            this.IsUnderContract = false;
        }


        //method to add employer to job

        public void AddEmployer(Employer employer)
        {
            this.Employer = employer;
            this.EmployerRefId = employer.EmployerId;

        }


    }





}
