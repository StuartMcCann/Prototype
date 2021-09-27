using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prototype.Data;
using Prototype.Models;
using Prototype.Service;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public JobController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            //creates a db objext for use in controller using dependency injection
            _db = db;
            _userManager = userManager;

        }
        #region PageNavAndCrud
        //view for candidates to view jobs 
        [Authorize(Roles = "Candidate")]
        public IActionResult Index()
        {

            return View();
        }


        [Authorize(Roles = "Employer")]
        //get for create
        public IActionResult Create()
        {
            //populate bag for dropdown menu 
            ViewBag.Skills = new SelectList(_db.Skills, "SkillId", "SkillName");

            return View();
        }

        //Post for create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            //get all skills 
            foreach (int skillId in job.SkillIds)
            {
                job.Skills.Add(_db.Skills.Where(s => s.SkillId == skillId).First());
            }
            var user = GetUser();
            int employerId = (int)user.EmployerId;
            Employer employer = _db.Employers.Where(e => e.EmployerId == employerId).First();
            job.AddEmployer(employer);

            //validation below 
            if (ModelState.IsValid)
            {
                _db.Jobs.Add(job);
                //save changes exexutes action to DB
                _db.SaveChanges();
                //needs changed to direct to edit 
                return RedirectToAction("JobMatch", job);

            }
            return View(job);

        }

        [Authorize(Roles = "Candidate")]
        //get by id method
        public IActionResult JobProfile(int id)
        {

            var job = JobHelper.GetJobProfile(_db, id);

            if (job == null)
            {
                return NotFound();
            }
            return View(job);

        }



        //GET for Edit
        [Authorize(Roles = "Employer")]
        public IActionResult Edit(int id)
        {

            ViewBag.Skills = new SelectList(_db.Skills, "SkillId", "SkillName");
            if (id == 0)
            {
                return NotFound();
            }
            var job = JobHelper.GetJobProfile(_db, id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        //Post for Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            var user = GetUser();
            job.EmployerRefId = (int)user.EmployerId;
            if (job.SkillIds != null)
            {
                var skillsToAdd = UpdateJobSkills(job); 
                job.Skills.Clear();
                job.Skills =skillsToAdd;

            }
            //required for validation
            ModelState.Remove("SkillIds"); 
            //validation below 
            if (ModelState.IsValid)
            {
                //update needs primary key to update
                _db.Jobs.Update(job);
                //save changes exexutes action to DB
                _db.SaveChanges();
                return RedirectToAction("Hub", "Employer");

            }
            return View(job);


        }

        public List<Skill> UpdateJobSkills(Job job)
        {
            var skillsToBeAdded = new List<Skill>();
            

            foreach (int skillId in job.SkillIds)
            {
                Skill skill = _db.Skills.Where(s => s.SkillId == skillId).First();
                //check if duplicate skill
                if (!job.Skills.Contains(skill))
                {
                    skillsToBeAdded.Add(skill);
                }

            }

            return skillsToBeAdded;
        }

        //GET for Delete 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var job = _db.Jobs.Find(id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        //Post for Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAction(int JobId)
        {
            var job = _db.Jobs.Find(JobId);
            if (job == null)
            {
                return NotFound();
            }
            //need to remoive associated likes 
            var likes = LikeHelper.GetLikesByJobId(_db, JobId);
            foreach (Like like in likes)
            {
                _db.Likes.Remove(like);
            }
            //Remove needs primary key to update
            _db.Jobs.Remove(job);
            //save changes exexutes action to DB
            _db.SaveChanges();
            return RedirectToAction("Hub", "Employer");

        }


        //job match page 
        [Authorize(Roles = "Employer")]
        public IActionResult JobMatch(Job job)
        {
            //change to jobprofile? and add more complex logic 
            var matchedCandidates = JobMatchHelper.JobMatchWithCandidates(_db, job);
            return View(matchedCandidates);

        }

        #endregion
        #region GetMethods

        public ActionResult GetJobsLikeThis(JobTitle jobTitle, int jobId)
        {
            var jobsLikeThis = JobHelper.GetJobsLikesThis(_db, jobTitle, jobId);

            return Json(jobsLikeThis);

        }

        public ActionResult GetJobsByEmployerId(int employerId)
        {
            var userJobs = JobHelper.GetUserJobs(_db, (int)employerId);
            return Json(userJobs);
            //return Json(new { data = userJobs });
        }

        public ActionResult GetJobsByUserId()
        {
            var user = GetUser();

            var userJobs = JobHelper.GetUserJobs(_db, (int)user.EmployerId);

            return Json(userJobs);
        }

        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user;
        }

        public List<JobProfile> GetAllLiveJobs()
        {
            var availableJobs = JobHelper.GetAllLiveJobs(_db);
            return availableJobs;

        }


        public List<JobProfile> GetJobsStartingSoon(JobTitle jobTitle)
        {
            var jobsClosingSoon = JobMatchHelper.GetJobsStartingSoon(jobTitle, _db);
            return jobsClosingSoon;

        }

        
        #endregion

    }
}
