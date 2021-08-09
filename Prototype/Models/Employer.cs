using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public String CompanyOverview { get; set; }
        public double Rating { get; set; }
        [Display(Name = "Company Logo")]
        public byte[] CompanyLogo { get; set; }

        //Forign Key for one to Many with Jobs
        public ICollection<Job> Jobs { get; set;  }

        /*Many to many With Candidates for Likes
        public ICollection<Candidate> Candidates { get; set; }*/
        public ICollection<Like> Likes { get; set; }
        //foreign key one to many with Contracts
        public ICollection<Contract> Contracts { get; set; }

        //one to many relationship with Application User - one employer can have any users associated with it 
        //public ApplicationUser ApplicationUser { get; set; }
    }
}
