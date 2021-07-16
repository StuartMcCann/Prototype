using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

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

        public IActionResult Index()
        {
           
            //below gets Jobs from db
            IEnumerable<Job> jobList = _db.Jobs;
            return View(jobList);
        }

        public List<JobProfile> GetJobsByUserID()
        {

            // getting user id using user manager 
            var userId = _userManager.GetUserId(User);
            //query db 
            var userJobs = (from j in _db.Jobs
                            join employers in _db.Employers on j.EmployerRefId
                            equals employers.EmployerId
                            join u in _db.Users on employers.EmployerId
                            equals u.EmployerId
                            join jobTitle in _db.JobTitle on
                            j.JobTitleRefId equals jobTitle.JobTitleId
                            where  u.Id == userId
                            select new JobProfile
                            {
                                JobID = j.JobId,
                                //remove title when normalise properly 
                                Title = j.JobTitle,
                                JobDescription = j.JobDescription,
                                UpperRate = j.UpperRate,
                                LowerRate = j.LowerRate,
                                JobTitle = jobTitle.Title,
                                CompanyName = employers.CompanyName,
                                Duration = j.Duration,
                                StartDate = j.StartDate,
                                Rating = employers.Rating

                            }).ToList();

            return userJobs; 
        }

        //Todo : get likes by user id 

        //todo: get messages by user id 

        //to do get contracts by user id 


        //get for create
        public IActionResult Create()
        {

            return View();
        }

        //Post for create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            //validation below 
            if (ModelState.IsValid)
            {
                _db.Jobs.Add(job);
                //save changes exexutes action to DB
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(job);
           


        }

        public ActionResult GetJobsLikeThis(string title)
        {
            

            var jobsLikeThis = (from j in _db.Jobs
                       join employers in _db.Employers on j.EmployerRefId
                       equals employers.EmployerId
                       join jobTitle in _db.JobTitle on
                       j.JobTitleRefId equals jobTitle.JobTitleId
                       where j.JobTitle == title
                       select new JobProfile
                       {
                           JobID = j.JobId,
                           //remove title when normalise properly 
                           Title = j.JobTitle, 
                           JobDescription = j.JobDescription,
                           UpperRate = j.UpperRate,
                           LowerRate = j.LowerRate,
                           JobTitle = jobTitle.Title,
                           CompanyName = employers.CompanyName,
                           Duration = j.Duration,
                           StartDate = j.StartDate,
                           Rating = employers.Rating

                       }).ToList();

            return Json(new { data = jobsLikeThis });

        }


        public ActionResult GetJobTitles()
        {
            List<JobTitle> jobTitles = _db.JobTitle.ToList();

            return Json(new { data = jobTitles }); 

        }

        public ActionResult GetJobSkills()
        {
            List<JobTitle> jobTitles = _db.JobTitle.ToList();

            return Json(new { data = jobTitles });

        }



        //get by id method
        public IActionResult JobProfile(int id)
        {


            // var job = _db.Jobs.Find(id);
            var job = (from j in _db.Jobs
                      join employers in _db.Employers on j.EmployerRefId
                      equals employers.EmployerId
                      join jobTitle in _db.JobTitle on
                      j.JobTitleRefId equals jobTitle.JobTitleId
                      where j.JobId == id
                      select new JobProfile
                      {
                         JobID= j.JobId,
                         Title = j.JobTitle,
                          JobDescription = j.JobDescription,
                         UpperRate = j.UpperRate,
                         LowerRate = j.LowerRate,
                         JobTitle =jobTitle.Title,
                         CompanyName =employers.CompanyName,
                         Duration =  j.Duration,
                         StartDate=  j.StartDate,
                         Rating=employers.Rating

                      }).ToList(); 



             if (job == null)
            {
                return NotFound();
            }
            return View(job);

        }

        

        //GET for Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Jobs.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //Post for Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            //validation below 
            if (ModelState.IsValid)
            {
                //update needs primary key to update
                _db.Jobs.Update(job);
                //save changes exexutes action to DB
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(job);


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
        public IActionResult DeleteAction(int? JobId)
        {
            var job = _db.Jobs.Find(JobId);
            if (job == null)
            {
                return NotFound();
            }

            //Remove needs primary key to update

            _db.Jobs.Remove(job);
            //save changes exexutes action to DB
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user;
        }


    }
}
