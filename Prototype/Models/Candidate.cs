﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Models
{
    public class Candidate
    {
        private const double MinimumWage = 8.21;

        [Key]
        public int CandidateID { get; set; }
        //remove level if enum works 


        public Boolean IsAvailable { get; set; }
        [NotMapped]
        private DateTime _AvailableFrom;
        [Required]
        public DateTime AvailableFrom
        {
            get
            {
                return _AvailableFrom;
            }
            set
            {
                int result = DateTime.Compare(value, DateTime.Now);
                //if availability is in future we set as not available
                if (result > 0)
                {
                    this.IsAvailable = false;
                }
                else
                {
                    // if in past we set as available
                    this.IsAvailable = true;
                }
                _AvailableFrom = value;
            }
        }

        //REMOVE SKILL
        public String Skill { get; set; }

        public double Rating { get; set; }
        [Required]
        [Range(MinimumWage, double.MaxValue, ErrorMessage = "Please select a value over national living wage")]
        public double Rate { get; set; }

        public Level LevelEnum { get; set; }
        //normalised JobTitle

        public JobTitle JobTitleEnum { get; set; }



        //foreign Key with Users table
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }


        //foreign key one to many with Likes 
        public ICollection<Like> Likes { get; set; }
        //foreign key one to many with Contracts
        public ICollection<Contract> Contracts { get; set; }
        //many to many with Skills
        public virtual ICollection<Skill> Skills { get; set; }
        [NotMapped]
        [Required]
        [MinLength(3, ErrorMessage = "Please select at least 3 skills")]
        public IEnumerable<int> SkillIds { get; set; }


        //default constructor 
        public Candidate()
        {
            this.Skills = new HashSet<Skill>();


        }




    }


}
