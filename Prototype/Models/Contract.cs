using Prototype.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Prototype.Models
{
    public class Contract
    {
        private const int MaxRating = 5;
        private const int MinRating = 1;
        private const double MinimumWage = 8.21;

        [Key]
        public int ContractId { get; set; }

        [NotMapped]
        private double _AgreedRate;
        [Required]
        [Range(MinimumWage, double.MaxValue, ErrorMessage = "Please select a value over national living wage")]
        public double AgreedRate
        {
            get { return _AgreedRate; }
            set
            {
                if (value >= MinimumWage)
                {
                    _AgreedRate = value;
                }
                else
                {
                    throw new ArgumentException("Agreed Rate Cannot be below Minimum Wage");
                }

            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsRatedByEmployer { get; set; }
        public bool IsRatedByCandidate { get; set; }
        public bool IsUnderContract { get; set; }
        //foreign key one to many with Candidate - one candidate has many contracts 
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        //foreign key one to many with Employer - one employer has many contracts 
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }
        //foreign key one to one with Job 
        public int JobId { get; set; }
        public virtual Job Job { get; set; }

        [NotMapped]
        private int _ContractRatingByCandidate;
        [Range(MinRating, MaxRating, ErrorMessage = "Please select a number between 1 and 5 inclusive")]
        public int? ContractRatingByCandidate
        {
            get { return _ContractRatingByCandidate; }
            set
            {
                if (value > 0 && value != null) { this.IsRatedByCandidate = true; }
                _ContractRatingByCandidate = (int)value;
            }
        }
        [NotMapped]
        private int _ContractRatingByEmployer;
        [Range(MinRating, MaxRating, ErrorMessage = "Please select a number between 1 and 5 inclusive")]
        public int? ContractRatingByEmployer
        {
            get { return _ContractRatingByEmployer; }
            set
            {
                if (value > 0 && value != null) { this.IsRatedByEmployer = true; }
                _ContractRatingByEmployer = (int)value;
            }
        }


        //Default constructor 
        public Contract()
        {
            //always sets this to true on creation 
            this.IsUnderContract = true;
        }
        //contructor with args 
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


        public void EndContract(DateTime endDate)
        {
            this.IsUnderContract = false;
            this.EndDate = endDate;
        }




    }

    public class ContractProfile : Contract
    {
        public string JobTitle { get; set; }
        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string EndDateDisplay { get; set; }


    }



}
