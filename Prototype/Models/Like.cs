using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public LikeType LikeType { get; set; }
        public int? JobId { get; set; }
        public Job Job { get; set; }
        public int? EmployerId { get; set; }
        public Employer Employer { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }



        //Foreign Keys to Job/Employer/Candidate One to Many Relationships
    }

    public enum LikeType
    {
        EmployerLikesCandidate, CandidateLikesJob, CandidateLikeEmployer
    }
}
