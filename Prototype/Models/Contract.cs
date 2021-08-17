using Prototype.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Prototype.Models
{
    public class Contract
    {


        [Key]
        public int ContractId { get; set; }
        public double AgreedRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsRatedByEmployer { get; set; }
        public bool IsRatedByCandidate { get; set; }
        public bool IsUnderContract { get; set; }
        //foreign key one to many with Candidate - one candidate has many contracts 
        //[ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        //foreign key one to many with Employer - one employer has many contracts 
        //[ForeignKey("Employer")]
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
        //foreign key one to one with Job 
        //[ForeignKey("Job")]
        public int JobId { get; set; }
        public virtual Job Job { get; set; }

        public int? ContractRatingByCandidate { get; set; }

        public int? ContractRatingByEmployer { get; set; }
        //add foreign keys for reviews?


        //Default constructor 
        public Contract()
        {
            //always sets this to true on creation 
            this.IsUnderContract = true;
        }
        //ontructor with args 
        public Contract(int jobId, int candidateId, DateTime startDate, double rate, ApplicationDbContext db)
        {
            this.JobId = jobId;            
            this.CandidateId = candidateId;         
            this.StartDate = startDate;
            this.AgreedRate = rate;
            this.IsUnderContract = true;
            this.IsRatedByEmployer = false;
            this.Candidate = db.Candidates.Where(c => c.CandidateID == candidateId).First();
            this.Job = db.Jobs.Where(j => j.JobId == jobId).First();
            this.EmployerId = this.Job.EmployerRefId;
            this.Employer = db.Employers.Where(c => c.EmployerId == this.Job.EmployerRefId).First();
            this.Candidate.ApplicationUser = db.Users.Where(u => u.Id == this.Candidate.UserId).First();



        }

        //could pass use now?
        public void EndContract(DateTime endDate)
        {
            this.IsUnderContract = false;
            this.EndDate = endDate;
            //this.EndDate = DateTime.UtcNow; 
        }

        


    }

    public class ContractProfile : Contract
    {
        public string JobTitle  { get; set; }
        public string FullName { get; set; }

        public string CompanyName { get; set; }




    }



}
