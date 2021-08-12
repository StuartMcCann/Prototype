using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Controllers
{
    public class ContractController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public ContractController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _db = applicationDbContext;
        }

        //may need to change to contractId = and change in redirect in create 
        public IActionResult Index(Contract contract)
        {


            contract.Candidate = _db.Candidates.Where(c => c.CandidateID == contract.CandidateId).First();
            contract.Job = _db.Jobs.Where(j => j.JobId == contract.JobId).First();
            contract.EmployerId = contract.Job.EmployerRefId;
            contract.Employer = _db.Employers.Where(c => c.EmployerId == contract.EmployerId).First();
            contract.Candidate.ApplicationUser = _db.Users.Where(u => u.Id == contract.Candidate.UserId).First();
            return View(contract);
        }
        //get for create 
        public IActionResult Create(int candidateId)
        {
            //get candidate details to create contract 
            var candidate = (from c in _db.Candidates
                             join u in _db.Users on
                             c.UserId equals u.Id
                             where c.CandidateID == candidateId
                             select new CandidateProfile
                             {
                                 CandidateID = c.CandidateID,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Rating = c.Rating,
                                 Rate = c.Rate,
                                 AvailableFrom = c.AvailableFrom,
                                 ProfilePicture = u.ProfilePicture
                             }).FirstOrDefault(); 

            return View(candidate);            
        }

        //post for create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int jobId, int candidateId, DateTime startDate, double rate)
        {
            Contract contract = new Contract(jobId, candidateId, startDate, rate, _db);
            if (ModelState.IsValid)
            {
                _db.Contracts.Add(contract);
                _db.SaveChanges();
                UpdateJobStatus(contract); 
                return RedirectToAction("Index",  contract  ); 
            }

            return View(candidateId); 

        }

        public void UpdateJobStatus(Contract contract)
        {
            contract.Job.IsLive = false;
            _db.Jobs.Update(contract.Job);
            _db.SaveChanges(); 
        }

        public List<Contract> GetContractsEmployerHub(int employerId)
        {

            // var contracts = _db.Contracts.Where(c => c.EmployerId == employerId).ToList();
            var contracts = (from c in _db.Contracts
                             where c.EmployerId == employerId
                             select new Contract
                             {
                                 ContractId = c.ContractId,
                                 AgreedRate = c.AgreedRate,
                                 StartDate = c.StartDate,
                                 EndDate = c.EndDate,
                                 IsUnderContract = c.IsUnderContract,
                                 Candidate = _db.Candidates.Where(c => c.CandidateID == c.CandidateID).First(),
                                 Employer = _db.Employers.Where(e => e.EmployerId == c.EmployerId).First(),
                                 Job = _db.Jobs.Where(j=> j.JobId == c.JobId).First(),
                                 //Candidate.ApplicationUser = _db.Users.Where(u => u.Id == Candidate.UserId), 
                                 IsRated = c.IsRated

                             }).ToList(); 
            return contracts; 
        }
    }
}
