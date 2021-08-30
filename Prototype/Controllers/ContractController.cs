using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using Prototype.Service;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [Authorize]
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
        [Authorize(Roles = "Employer")]
        public IActionResult Create(int candidateId)
        {
            var candidate = CandidateHelper.GetCandidateProfile(_db, candidateId);

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
                //set cand isavailable?
                return RedirectToAction("Index", contract);
            }

            return View(candidateId);

        }

        //change to in job model?
        public void UpdateJobStatus(Contract contract)
        {
            contract.Job.IsLive = false;
            contract.Job.IsUnderContract = true;
            if (ModelState.IsValid)
            {
                _db.Jobs.Update(contract.Job);
                _db.SaveChanges();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompleteContract(int contractId, int? rating, DateTime endDate)
        {
            var contract = _db.Contracts.Where(c => c.ContractId == contractId).First();
            //set to false as completed
            contract.IsUnderContract = false;
            //set end date from params 
            contract.EndDate = endDate;
            //set rating         

            if (rating != 0)
            {
                // contract.IsRatedByEmployer = true;
                contract.ContractRatingByEmployer = rating;
            }
            if (ModelState.IsValid)
            {
                _db.Contracts.Update(contract);
                _db.SaveChanges();
            }
            //update average rating for candidate 
            AddContractRatingForCandidate(contract.CandidateId);

            return RedirectToAction("Index", contract);

        }

        public void AddContractRatingForCandidate(int candidateId)
        {
            //get all rated contracts for candidate and count 
            var allRatedContracts = _db.Contracts.Where(c => c.CandidateId == candidateId && c.IsRatedByEmployer == true).Select(c => c.ContractRatingByEmployer).ToList();
            //get average 
            var averageCandidateRating = allRatedContracts.Average();
            //update candidate in DB
            var candidate = _db.Candidates.Where(c => c.CandidateID == candidateId).First();
            candidate.Rating = Math.Round((double)averageCandidateRating);
            //clear skills to avoid conflict
            candidate.Skills.Clear();
            if (ModelState.IsValid)
            {
                _db.Candidates.Update(candidate);
                _db.SaveChanges();
            }



        }

        [HttpPost]

        public ActionResult RateContractByCandidate(int contractId, int rating)
        {
            var contract = _db.Contracts.Where(c => c.ContractId == contractId).First();
            contract.ContractRatingByCandidate = rating;
            if (ModelState.IsValid)
            {
                _db.Contracts.Update(contract);
                _db.SaveChanges();
            }
            AddContractRatingForEmployer(contract.EmployerId);

            return RedirectToAction("Index", contract);


        }

        public void AddContractRatingForEmployer(int employerId)
        {
            //get all rated contracts for candidate and count 
            var allRatedContracts = _db.Contracts.Where(c => c.EmployerId == employerId && c.IsRatedByCandidate == true).Select(c => c.ContractRatingByCandidate).ToList();
            //get average 
            var averageEmployerRating = allRatedContracts.Average();
            //update candidate in DB
            var employer = _db.Employers.Where(e => e.EmployerId == employerId).First();
            employer.Rating = Math.Round((double)averageEmployerRating);
            if (ModelState.IsValid)
            {
                _db.Employers.Update(employer);
                _db.SaveChanges();
            }

        }




        //post for create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRate(double rate, int contractId)
        {
            var contract = _db.Contracts.Where(c => c.ContractId == contractId).First();

            contract.AgreedRate = rate;
            if (ModelState.IsValid)
            {
                _db.Contracts.Update(contract);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", contract);

        }

        public ActionResult GetContractsEmployerHub(int employerId)
        {

            // var contracts = _db.Contracts.Where(c => c.EmployerId == employerId).ToList();
            var contracts = ContractHelper.GetContractsForEmployerHub(_db, employerId); 
            //return contracts; 
            return Json(new { data = contracts });
        }

        public ActionResult GetContractsCandidateHub(int candidateId)
        {

            // var contracts = _db.Contracts.Where(c => c.EmployerId == employerId).ToList();
            var contracts = ContractHelper.GetContractForCandidateHub(_db, candidateId); 
            //return contracts; 
            return Json(new { data = contracts });
        }



        public ActionResult GetContractsToRate(int id)
        {
            var contracts = new List<ContractProfile>();
            if (User.IsInRole("Candidate"))
            {
                contracts = ContractHelper.GetContractToRateCandidate(_db, id); 

            }
            else if (User.IsInRole("Employer"))
            {
                //contracts = _db.Contracts.Where(c => c.EmployerId == id && !c.IsRatedByEmployer).ToList();

            }

            return Json(contracts);
        }




        public IActionResult EmployerHubRedirect(int contractId)
        {
            var contract1 = _db.Contracts.Where(c => c.ContractId == contractId).First();
            return RedirectToAction("Index", contract1);
        }
    }
}
