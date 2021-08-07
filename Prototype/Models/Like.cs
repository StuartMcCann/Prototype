using Prototype.Data;
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
            this.Employer = (Employer)db.Employers.Where(e => e.EmployerId == employerId).First();
            this.Candidate = (Candidate)db.Candidates.Where(c => c.CandidateID == candidateId).First();

        }

        public Like(LikeType likeType, int employerId, int candidateId , int jobId, ApplicationDbContext db)
        {
            this.LikeType = likeType;
            this.EmployerId = employerId;
            this.CandidateId = candidateId;
            this.JobId = jobId;
            this.Employer = (Employer)db.Employers.Where(e => e.EmployerId == employerId).First();
            this.Candidate = (Candidate)db.Candidates.Where(c => c.CandidateID == candidateId).First();
            this.Job = (Job)db.Jobs.Where(j => j.JobId == jobId).First(); 

        }
        



        //Foreign Keys to Job/Employer/Candidate One to Many Relationships
    }

   

    public enum LikeType
    {
        EmployerLikesCandidate, CandidateLikesJob, CandidateLikesEmployer
    }

    public class EmployerLike
    {

        public int CandidateId { get; set; }
        public int? EmployerId { get; set; }

        public LikeType LikeType { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }






    }
}
