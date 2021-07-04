using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Employer:User 
    {
        [Key]
        public int EmployerId { get; set; }
        public String CompanyName { get; set; }

        //Forign Key for one to Many with Jobs
        public ICollection<Job> Jobs { get; set;  }

    }
}
