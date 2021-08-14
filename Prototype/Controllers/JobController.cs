using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
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

        public IActionResult Index()
        {

            //below gets Jobs from db
            IEnumerable<Job> jobList = _db.Jobs;
            return View(jobList);
        }

        public ActionResult GetJobsByUserID()
        {

            // getting user id using user manager 
            // var userId = _userManager.GetUserId(User);
            var user = GetUser();
            var employerId = user.EmployerId;

            var userJobs = (from j in _db.Jobs
                            join u in _db.Users
                            on j.EmployerRefId equals u.EmployerId
                            where j.EmployerRefId == employerId
                            && j.IsLive == true
                            select new JobProfile
                            {
                                JobId = j.JobId,
                                JobTitleEnum = j.JobTitleEnum,
                                StartDate = j.StartDate,
                                UpperRate = j.UpperRate,
                                LowerRate = j.LowerRate,
                                Duration = j.Duration,
                                JobDescription = j.JobDescription,
                                IsFilled = j.IsFilled,
                                IsLive = j.IsLive,
                                IsUnderContract = j.IsUnderContract,
                                EmployerRefId = j.EmployerRefId,
                                JobTitle = j.JobTitleEnum.GetDisplayName(), 
                                Level = j.LevelEnum.GetDisplayName()


                            }).ToList();

            return Json(new { data = userJobs });
        }


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
                return RedirectToAction("JobMatch", new { job = job });

            }
            return View(job);



        }

        public IActionResult JobMatch(Job job)
        {
            //change to jobprofile? and add more complex logic 
            var matchedCandidates = (from c in _db.Candidates
                                     where c.IsAvailable == true
                                    select new CandidateProfile
                                    {
                                        CandidateID = c.CandidateID, 
                                        ApplicationUser = c.ApplicationUser, 
                                        LevelEnum = c.LevelEnum,
                                        Rating = c.Rating, 
                                        JobTitleEnum = c.JobTitleEnum, 
                                        Rate = c.Rate, 
                                        AvailableFrom = c.AvailableFrom, 
                                        //Skill - c.Skill

                                    }).ToList();

            return View(matchedCandidates);

        }

        public ActionResult GetJobsLikeThis(JobTitle jobTitle)
        {


            var jobsLikeThis = (from j in _db.Jobs
                                    //join employers in _db.Employers on j.EmployerRefId
                                    //equals employers.EmployerId

                                where j.JobTitleEnum == jobTitle
                                && j.IsLive == true
                                select new JobProfile
                                {
                                    JobId = j.JobId,

                                    JobDescription = j.JobDescription,
                                    UpperRate = j.UpperRate,
                                    LowerRate = j.LowerRate,
                                    JobTitleEnum = j.JobTitleEnum,
                                    CompanyName = j.Employer.CompanyName,
                                    Duration = j.Duration,
                                    StartDate = j.StartDate,
                                    Rating = j.Employer.Rating,
                                    Employer = j.Employer,
                                    JobTitle = j.JobTitleEnum.GetDisplayName(), 
                                    Level = j.LevelEnum.GetDisplayName()

                                }).ToList();

            return Json(new { data = jobsLikeThis });

        }


      



        //get by id method
        public IActionResult JobProfile(int id)
        {


            // var job = _db.Jobs.Find(id);
            var job = (from j in _db.Jobs
                       join employers in _db.Employers on j.EmployerRefId
                       equals employers.EmployerId
                       join u in _db.Users
                       on j.EmployerRefId equals u.EmployerId
                       //join jobTitle in _db.JobTitle on
                       //j.JobTitleRefId equals jobTitle.JobTitleId
                       where j.JobId == id
                       select new JobProfile
                       {
                           JobId = j.JobId,
                           JobDescription = j.JobDescription,
                           UpperRate = j.UpperRate,
                           LowerRate = j.LowerRate,
                           JobTitleEnum = j.JobTitleEnum,
                           CompanyName = employers.CompanyName,
                           Duration = j.Duration,
                           StartDate = j.StartDate,
                           Rating = employers.Rating,
                           EmployerId = employers.EmployerId,
                           UserId = u.Id

                       }).FirstOrDefault();



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

        public List<JobProfile> GetAllLiveJobs()
        {


            var availableJobs = (from j in _db.Jobs
                                 join e in _db.Employers on 
                                j.EmployerRefId equals e.EmployerId
                                 where j.IsLive == true
                                 select new JobProfile
                                 {
                                     JobId = j.JobId, 
                                     JobTitle = j.JobTitleEnum.GetDisplayName(),
                                     LowerRate = j.LowerRate,
                                     UpperRate = j.UpperRate,
                                     Duration = j.Duration,
                                     JobDescription = j.JobDescription,
                                     Level = j.LevelEnum.GetDisplayName(),
                                     EmployerId = j.EmployerRefId,
                                     //add skills required                                       
                                     CompanyName = e.CompanyName, 
                                     Rating = e.Rating

                                 }).ToList();


            return availableJobs;

        }


    }
}
