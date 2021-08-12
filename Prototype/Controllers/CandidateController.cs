using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Enums;
using Prototype.Helpers;
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

            // change to job title??
            var canidatesLikethis = (from c in _db.Candidates
                                where c.Skill==skill && c.CandidateID!=id
                                
                                select new CandidateProfile
                                {
                                    
                                    LevelEnum = c.LevelEnum, 
                                    Rating = c.Rating, 
                                    Rate = c.Rate, 
                                    Skill = c.Skill, 
                                    CandidateID = c.CandidateID, 
                                    

                                }).ToList();

            return Json(new { data = canidatesLikethis });

        }

        //get for edit 
        public IActionResult Edit()
        {
            //get the application user details 
            var user = GetUser();
            //update this

            var userId = user.Id;
            var candidate = GetCandidateDetailsByUser(userId); 
            if (candidate.Count() == 0)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return View(candidate);
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

            var user = GetUser();
            var userId = user.Id;
            candidate.ApplicationUser = user;
            candidate.UserId = userId; 
           // candidate.UserId = userId;
            // add user to candidate 
            //candidate = new Candidate(candidate, userId, _db); 
            //validation below 
            if (ModelState.IsValid)
            {

                //add employer created to DB
                _db.Candidates.Add(candidate);
                //save changes exexutes action to DB
                _db.SaveChanges();

                
                return RedirectToAction("Edit");

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

            //TODO - select query to create cand profile list where available = true

            IEnumerable<CandidateProfile> candidateList = ((from c in _db.Candidates
                                                         //join r in _db.Reviews on c.CandidateId equals
                                                         // r.CandidateRefId
                                                     join u in _db.Users
                                                     on c.UserId equals u.Id
                                                     where c.IsAvailable
                                                     select new CandidateProfile
                                                     {
                                                         CandidateID = c.CandidateID,
                                                         LevelEnum = c.LevelEnum,
                                                         Skill = c.Skill,
                                                         Rating = c.Rating,
                                                         Rate = c.Rate,
                                                         UserId = u.Id,
                                                         ProfilePicture = u.ProfilePicture,
                                                         FirstName = u.FirstName,
                                                         LastName = u.LastName,

                                                         //for loop here to add to list of reviews or can do ajax call on page t print 

                                                     })).ToList(); 
            
            return View(candidateList);
        }

        public IActionResult CandidateProfile(int id)
        {
            

            var candidate = ((from c in _db.Candidates
                                  //join r in _db.Reviews on c.CandidateId equals
                                  // r.CandidateRefId
                              join u in _db.Users
                              on c.UserId equals u.Id
                              where c.CandidateID == id
                              select new CandidateProfile
                              {
                                  CandidateID = c.CandidateID,
                                  LevelEnum = c.LevelEnum,
                                  Skill = c.Skill,
                                  Rating = c.Rating,
                                  Rate = c.Rate, 
                                  UserId = u.Id, 
                                  ProfilePicture = u.ProfilePicture, 
                                  FirstName = u.FirstName, 
                                  LastName = u.LastName, 
                                 

                                  //for loop here to add to list of reviews or can do ajax call on page t print 

                              })).FirstOrDefault();   



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

        public List<Candidate> GetCandidateDetailsByUser(string userId)
        {
            var candidate = (from c in _db.Candidates
                             where c.UserId == userId
                             select new Candidate
                             {
                                 
                                 LevelEnum = c.LevelEnum,
                                 Rating = c.Rating,
                                 Rate = c.Rate,
                                 Skill = c.Skill,
                                 CandidateID = c.CandidateID,
                                 

                             }).ToList();

            return candidate; 


        }
        

    }



}
