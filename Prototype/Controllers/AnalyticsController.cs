using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prototype.Data;
using Prototype.Models;
using Prototype.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Controllers
{
    public class AnalyticsController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnalyticsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            //creates a db objext for use in controller using dependency injection
            _db = db;
            _userManager = userManager;
        }



        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Candidate")]
        public IActionResult AnalyticsForCandidate()
        {
            try
            {
                         
                var userId = _userManager.GetUserId(User);
            var candidate = _db.Candidates.Where(c => c.UserId == userId).FirstOrDefault();
                
            return View(candidate);

            }
            catch (Exception ex)
            {
                return View("Error"); 
            }
        }



        [Authorize(Roles ="Employer")]
        public IActionResult AnalyticsForEmployer()
        {

            try { 
            var user = _db.Users.Where(u=> u.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var employer = _db.Employers.Where(e => e.EmployerId == user.EmployerId).FirstOrDefault();
            ViewBag.Skills = new SelectList(_db.Skills, "SkillId", "SkillName");

            return View(employer);
            }catch(Exception ex)
            {
                return View("Error"); 
            }
        }


        public ActionResult AnalysisByCandidateJobTitle(JobTitle jobTitle)
        {
            var analysedResults = AnalyticsHelper.GetRateAnalysisForJobTitle(_db, jobTitle);

            return Json(analysedResults) ; 

        }

        public ActionResult InDemandSkills()
        {
            var inDemandSkills = AnalyticsHelper.InDemandSkills(_db);

            return Json(inDemandSkills); 
        }

        public ActionResult PopularSkills()
        {
            var popularSkills = AnalyticsHelper.PopularSkills(_db);

            return Json(popularSkills);
        }

        public ActionResult GetTopRatedEmployers()
        {
            var topRatedEmployers = AnalyticsHelper.GetTopRatedEmployers(_db);

            return Json(topRatedEmployers); 
        }

        public ActionResult GetJobTitleData()
        {
            var jobTitleData = AnalyticsHelper.GetJobsTitlesCount(_db);

            return Json(jobTitleData); 
        }

        public ActionResult GetJobTitlesOfAllCandidates()
        {

            var jobTitleData = AnalyticsHelper.GetJobsTitlesOfAvailableCandidates(_db);

            return Json(jobTitleData); 
        }

        public ActionResult GetBespokeCandidateData(int candidateId)
        {
            
            var analysedData = AnalyticsHelper.GetBespokeCandidateData(_db, candidateId);
            return Json(analysedData); 


        }

        public ActionResult GetBespokeEmployerData(int employerId)
        {

            var analysedData = AnalyticsHelper.GetBespokeEmployerData(_db, employerId);
            return Json(analysedData);


        }
    }
}
