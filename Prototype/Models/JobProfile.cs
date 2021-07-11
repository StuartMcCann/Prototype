using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class JobProfile
    {

        public int JobID { get; set; }
        //remove below when normalisation completed 
        public string Title { get; set; }
        public String JobDescription { get; set; }
        public double UpperRate { get; set; }
        public double LowerRate { get; set; }
        public String JobTitle { get; set; }
        public String CompanyName { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }

        public double Rating { get; set; }
    }
}
