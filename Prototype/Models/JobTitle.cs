using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class JobTitle
    {
        [Key]
        public int JobTitleId { get; set; }
        public String Title { get; set; }

        //foreign Keys 
        [ForeignKey("JobTitleRefId")]
        public ICollection<Job> Jobs { get; set; }
    }
}
