using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class CandidateProfile
    {
        //join properties need to be added when tables normalised/ full database deved
        public int CandidateId { get; set; }
        public String Name { get; set; }
        public String Level { get; set; }

        public String Skill { get; set; }
         public Level LevelEnum { get; set; }
        public double Rating { get; set; }
        public double Rate { get; set; }
    }
}
