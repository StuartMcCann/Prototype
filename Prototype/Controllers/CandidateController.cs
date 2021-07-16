using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager; 

        public CandidateController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            //creates a db objext for use in controller using dependency injection
            _db = db;
            _userManager = userManager; 
        }

        //can change to have level and skills - pass skills as a List?
        public ActionResult GetCandidatesLikeThis(string skill, int id)
        {
            var canidatesLikethis = (from c in _db.Candidates
                                where c.Skill == skill && c.CandidateID!=id
                                select new CandidateProfile
                                {
                                    Name = c.Name, 
                                    Level = c.Level, 
                                    Rating = c.Rating, 
                                    Rate = c.Rate, 
                                    Skill = c.Skill, 
                                    CandidateId = c.CandidateID, 
                                    LevelEnum = c.LevelEnum

                                }).ToList();

            return Json(new { data = canidatesLikethis });

        }

        //get for edit 
        public IActionResult Edit()
        {
            //get the application user details 
            var user = GetUser();
            //update this 
            var employerId = user.EmployerId;
            Employer employer = _db.Employers.Find(employerId);
            if (employer == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return View(employer);
            }
        }

        //post for edit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(Candidate candidate)
        {

            if (ModelState.IsValid)
            {
                
                _db.Candidates.Update(candidate);
                _db.SaveChanges();
                return View(candidate);
            }
            else
            {
                return View(candidate);
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Candidate candidate)
        {
            //validation below 
            if (ModelState.IsValid)
            {

                //add employer created to DB
                _db.Candidates.Add(candidate);
                //save changes exexutes action to DB
                _db.SaveChanges();

                //add forign keys here?
                //var user = GetUser();
                //user.EmployerId = candidate.EmployerId;
                //_db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(candidate);

        }





        //get for create
        public IActionResult Create()
        {
            return View();
        }


        public IActionResult Index()
        {
           // List<Candidate> list =  _db.Candidates.Where(s => s.Level == "Entry").OrderByDescending(c=>c.Rating).ToList<Candidate>(); 
            
            //below gets candidates from db
            IEnumerable<Candidate> candidateList = _db.Candidates;
            return View(candidateList);
        }

        public IActionResult CandidateProfile(int id)
        {

            var candidate = (from c in _db.Candidates
                                 //join r in _db.Reviews on c.CandidateId equals
                                 // r.CandidateRefId
                            where c.CandidateID == id
                             select new CandidateProfile
                             {
                                 CandidateId = c.CandidateID,
                                 Name = c.Name,
                                 Level = c.Level,
                                 Skill = c.Skill,
                                 Rating = c.Rating,
                                 Rate = c.Rate
                                 //for loop here to add to list of reviews or can do ajax call on page t print 

                             }).ToList();



            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        //get for hub
        public IActionResult Hub()
        {
            return View();
        }


        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user;
        }


    }



}
