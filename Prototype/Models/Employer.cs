using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Prototype.Models
{
    public class Employer
    {


        [Key]
        public int EmployerId { get; set; }
        [Required]
        [DisplayName("Company Name")]
        public String CompanyName { get; set; }
        [DisplayName("Company Overview")]
        [Required]
        [StringLength(maximumLength: 1000, MinimumLength = 100, ErrorMessage = "Company overview must be between 100 - 1000 characters")]
        public String CompanyOverview { get; set; }
        public double Rating { get; set; }
        [Display(Name = "Company Logo")]
        public byte[] CompanyLogo { get; set; }

        //Forign Key for one to Many with Jobs
        public ICollection<Job> Jobs { get; set; }
        public ICollection<Like> Likes { get; set; }
        //foreign key one to many with Contracts
        public ICollection<Contract> Contracts { get; set; }

    }
}
