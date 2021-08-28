﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult AnalyticsForCandidate()
        {
            var userId = _userManager.GetUserId(User);
            var candidate = _db.Candidates.Where(c => c.UserId == userId).FirstOrDefault(); 

            return View(candidate); 
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

        public ActionResult GetBespokeCandidateData(int candidateId)
        {
            
            var likes = AnalyticsHelper.GetBespokeCandidateData(_db, candidateId);
            return Json(likes); 


        }
    }
}
