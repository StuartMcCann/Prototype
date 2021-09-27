using System;
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
                //returns 1 if in future/ 0 if todays date/ -1 if in past 
                int result = DateTime.Compare(value, DateTime.Now);
                //date time is in future therefore set available to false 
                if (result > 0)
                {
                    this.IsAvailable = false;
                }
                else
                {
                    // if in past or todays date  set as available
                    this.IsAvailable = true;
                }
                _AvailableFrom = value;
            }
        }

        [NotMapped]
        private double _Rating;

        public double Rating
        {

            get
            {
                return _Rating;

            }
            set
            {
                if (value >= 0 && value <= 5)
                {
                    _Rating = value;
                }
                else
                {
                    throw new ArgumentException("Rating cannot be less than 0 or grater than 5");
                }


               ;
            }
        }

        [NotMapped]
        private double _Rate;
        [Required]
        [Range(MinimumWage, double.MaxValue, ErrorMessage = "Please select a value over national living wage")]
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (value >= MinimumWage)
                {
                    _Rate = value;
                }
                else
                {
                    throw new ArgumentException("Rate Cannot be below Minimum Wage");
                }

            }
        }

        public Level LevelEnum { get; set; }
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
