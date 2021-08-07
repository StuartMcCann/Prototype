using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class JobProfile :Job
    {

       
        //remove below when normalisation completed 
        public string Title { get; set; }
        
        public  int EmployerId { get; set; }
        public String CompanyName { get; set; }
        public string UserId { get; set; }


        public double? Rating { get; set; }
    }
}
