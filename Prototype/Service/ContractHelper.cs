using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Service
{
    public class ContractHelper
    {
        //gets all contracts for an employer 
        public static List<ContractProfile> GetContractsForEmployerHub(ApplicationDbContext _db, int employerId)
        {
            return (from c in _db.Contracts
                    where c.EmployerId == employerId
                   
                    select new ContractProfile
                    {
                        ContractId = c.ContractId,
                        AgreedRate = c.AgreedRate,
                        StartDate = c.StartDate,
                        IsUnderContract = c.IsUnderContract,
                        FullName = GetFullName(c.CandidateId, _db),
                        EmployerId = c.EmployerId,
                        JobTitle = _db.Jobs.Where(j => j.JobId == c.JobId).First().JobTitleEnum.GetDisplayName(),
                        IsRatedByEmployer = c.IsRatedByEmployer

                    }).ToList();
        }
        //gets all contracts for a candidate 
        public static List<ContractProfile> GetContractForCandidateHub(ApplicationDbContext _db, int candidateId)
        {
            return (from c in _db.Contracts
                    where c.CandidateId == candidateId
                    select new ContractProfile
                    {
                        ContractId = c.ContractId,
                        AgreedRate = c.AgreedRate,
                        StartDate = c.StartDate,
                        IsUnderContract = c.IsUnderContract,
                        FullName = GetFullName(c.CandidateId, _db),
                        JobTitle = _db.Jobs.Where(j => j.JobId == c.JobId).First().JobTitleEnum.GetDisplayName(),
                        IsRatedByEmployer = c.IsRatedByEmployer,
                        CompanyName = c.Employer.CompanyName,
                        EmployerId = c.Employer.EmployerId,
                        CandidateId = c.CandidateId
                    }).ToList();
        }

        //gets contracts not yet rated for a candidate 
        public static List<ContractProfile> GetContractToRateCandidate(ApplicationDbContext _db, int candidateId)
        {
            return (from c in _db.Contracts
                    where c.CandidateId == candidateId && c.IsRatedByCandidate == false && c.IsRatedByEmployer == true
                    select new ContractProfile

                    {
                        ContractId = c.ContractId,
                        JobId = c.JobId,
                        JobTitle = _db.Jobs.Where(j => j.JobId == c.JobId).First().JobTitleEnum.GetDisplayName(),
                        EmployerId = c.EmployerId,
                        CompanyName = _db.Employers.Where(e => e.EmployerId == c.EmployerId).First().CompanyName,
                        EndDateDisplay = c.EndDate.ToShortDateString(),
                        AgreedRate = c.AgreedRate

                    }).ToList();
        }


        //gets full name for a candidate 
        public static string GetFullName(int candidateId, ApplicationDbContext db)
        {
            Candidate candidate = db.Candidates.Where(c => c.CandidateID == c.CandidateID).First();
            var user = db.Users.Where(u => u.Id == candidate.UserId).First();
            var fullName = user.FirstName + " " + user.LastName;
            return fullName;


        }

    }
}
