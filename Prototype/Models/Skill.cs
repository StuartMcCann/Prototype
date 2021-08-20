using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Prototype.Models
{
    public class Skill
    {

        [Key]
        public int SkillId { get; set; }
        public String SkillName { get; set; }
        //many to many with Job
        public virtual ICollection<Job> Jobs { get; set; }
        //Many to Many with Candidates 
        public virtual ICollection<Candidate> Candidates { get; set; }
       

        public Skill()
        {
            this.Jobs = new HashSet<Job>();
            this.Candidates = new HashSet<Candidate>();
        }
    }
}
