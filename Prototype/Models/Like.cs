using Prototype.Data;
using Prototype.Enums;
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

        public Like()
        {

        }

        public Like(LikeType likeType,  int employerId, int candidateId, ApplicationDbContext db)
        {
            this.LikeType =likeType;
            this.EmployerId = employerId;
            this.CandidateId = candidateId;
            this.Employer = db.Employers.Where(e => e.EmployerId == employerId).First();
            this.Candidate = db.Candidates.Where(c => c.CandidateID == candidateId).First();

        }

        public Like(LikeType likeType, int employerId, int candidateId , int jobId, ApplicationDbContext db)
        {
            this.LikeType = likeType;
            this.EmployerId = employerId;
            this.CandidateId = candidateId;
            this.JobId = jobId;
            this.Employer = db.Employers.Where(e => e.EmployerId == employerId).First();
            this.Candidate = db.Candidates.Where(c => c.CandidateID == candidateId).First();
            this.Job = db.Jobs.Where(j => j.JobId == jobId).First(); 

        }
        



        
    }

    public enum LikeType
    {
        EmployerLikesCandidate, CandidateLikesJob, CandidateLikesEmployer
    }


    public class EmployerLike:Like
    {

        
        public string FirstName { get; set; }

        public string LastName { get; set; }






    }
}
