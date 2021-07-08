using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _db; 

        public JobController(ApplicationDbContext db)
        {
            //creates a db objext for use in controller using dependency injection
            _db = db;  
        }

        public IActionResult Index()
        {
           
            //below gets Jobs from db
            IEnumerable<Job> jobList = _db.Jobs;
            return View(jobList);
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

        //get by id method
        public IActionResult JobProfile(int id)
        {


            // var job = _db.Jobs.Find(id);
            var job = (from j in _db.Jobs
                      join employers in _db.Employers on j.EmployerRefId
                      equals employers.EmployerId
                      join jobTitle in _db.JobTitle on
                      j.JobTitleRefId equals jobTitle.JobTitleId
                      select new JobProfile
                      {
                         JobID= j.JobId,
                         JobDescription= j.JobDescription,
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


    }
}
