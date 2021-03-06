using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class JobProfile :Job
    {

        public  int EmployerId { get; set; }
        public String CompanyName { get; set; }
        public string UserId { get; set; }
        public string JobTitle { get; set; }

        public string Level { get; set; }

        public double? Rating { get; set; }
        public string DisplayStartDate { get; set; }
    }
}
