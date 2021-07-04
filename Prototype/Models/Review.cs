using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public double Rating { get; set; }

        public String ReviewText { get; set; }

        //Foreign Key With Candidate (Eelationship - one candidate has many reviews)
        public int CandidateRefId { get; set;  }
        public Candidate Candidate { get; set; }


    }
}
