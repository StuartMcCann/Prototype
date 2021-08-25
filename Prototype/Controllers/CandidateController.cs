using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prototype.Data;
using Prototype.Enums;
using Prototype.Helpers;
using Prototype.Models;
using System;
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
            var candidatesLikeThis = (from c in _db.Candidates
                                      join u in _db.Users on
                                      c.UserId equals u.Id
                                      where c.Skill== skill && c.CandidateID != id
                                      && c.IsAvailable == true
                                      select new CandidateProfile
                                      {
                                         
                                          FirstName = u.FirstName, 
                                          LastName = u.LastName,
                                          LevelEnum = c.LevelEnum,
                                          Rating = c.Rating,
                                          Rate = c.Rate,
                                          Skill = c.Skill,
                                          CandidateID = c.CandidateID,
                                          Level = c.LevelEnum.GetDisplayName(), 
                                          Skills = c.Skills


                                      }).ToList();
            //return candidatesLikeThis; 
            return Json(new { data = candidatesLikeThis });

        }

        //get for edit 
        public IActionResult Edit()
        {
            //get the application user details 
            var user = GetUser();
             var userId = user.Id;
            var candidate = GetCandidateDetailsByUser(userId); 
            //making sure a user has a candidate profile created 
            if (candidate == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                //view bag for skills dropdowns 
                ViewBag.Skills = new SelectList(_db.Skills, "SkillId", "SkillName");
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
            foreach (int skillId in candidate.SkillIds)
            {
                candidate.Skills.Add(_db.Skills.Where(s => s.SkillId == skillId).First());
            }

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
            //view bag for skills dropdowns 
            ViewBag.Skills = new SelectList(_db.Skills, "SkillId", "SkillName");
            return View();
        }


        public IActionResult Index()
        {

            return View();
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
                                 Skills = c.Skills, 
                                 Likes = c.Likes, 
                                 Contracts = c.Contracts

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
            var user = GetUser();

            var candidate = _db.Candidates.Where(c => c.UserId == user.Id).FirstOrDefault(); 
            if (candidate != null)
            {
                return View(candidate);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }


        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user;
        }

        public CandidateProfile GetCandidateDetailsByUser(string userId)
        {
            var candidate = (from c in _db.Candidates
                             where c.UserId == userId
                             select new CandidateProfile
                             {

                                 LevelEnum = c.LevelEnum,
                                 Rating = c.Rating,
                                 Rate = c.Rate,
                                 CandidateID = c.CandidateID,
                                 Skills = c.Skills,
                                 Level = c.LevelEnum.GetDisplayName(),
                                 JobTitle = c.JobTitleEnum.GetDisplayName(),
                                 Likes = c.Likes,
                                 IsAvailable = c.IsAvailable,
                                 AvailableFrom = c.AvailableFrom,
                                 UserId = userId,
                                 ApplicationUser = _db.Users.Where(u => u.Id == userId).FirstOrDefault(), 
                                 JobTitleEnum = c.JobTitleEnum, 
                                 Contracts = c.Contracts
                               
                             }).FirstOrDefault();

           

            return candidate; 


        }

        public ActionResult UpdateCandidateSkill(List<int> skillsIds)
        {
            var user = GetUser();
            var candidate = GetCandidateDetailsByUser(user.Id);
            var skillsToBeAdded = new List<Skill>(); 

           
            foreach(int skillId in skillsIds)
            {
                Skill skill = _db.Skills.Where(s => s.SkillId == skillId).First();
                //check if duplicate skill
                if (!candidate.Skills.Contains(skill)){
                    skillsToBeAdded.Add(skill); 
                }              
                 
               
           }
            //clear candidate skills to avoid duplicate insert error 
            candidate.Skills.Clear();
            candidate.Skills = skillsToBeAdded; 

            _db.Candidates.Update(candidate);
            _db.SaveChanges(); 

            return RedirectToAction("Edit"); 


        }

        public List<Skill> GetSkillsForCandidateUpdate()
        {
            return _db.Skills.ToList(); 
        }

       public ActionResult UpdateCandidateJobTitle(JobTitle jobtitle)
        {
            var user = GetUser();
            var candidate = GetCandidateDetailsByUser(user.Id);
            //set jobtitleenum
            candidate.JobTitleEnum = jobtitle;
            //need to clear skills so no conlfict 
            candidate.Skills.Clear(); 
            //update and save changes 

            _db.Candidates.Update(candidate);
            _db.SaveChanges(); 


            return RedirectToAction("Edit"); 
        }


        public ActionResult UpdateCandidateLevel(Level level)
        {
            var user = GetUser();
            var candidate = GetCandidateDetailsByUser(user.Id);
            //set jobtitleenum
        
            candidate.LevelEnum = level;
            //need to clear skills so no conlfict 
            candidate.Skills.Clear();
            //update and save changes 

            _db.Candidates.Update(candidate);
            _db.SaveChanges();


            return RedirectToAction("Edit");
        }

        public ActionResult UpdateCandidateRate(double rate)
        {
            var user = GetUser();
            var candidate = GetCandidateDetailsByUser(user.Id);
            //set jobtitleenum

            candidate.Rate = rate;
            //need to clear skills so no conlfict 
            candidate.Skills.Clear();
            //update and save changes 

            _db.Candidates.Update(candidate);
            _db.SaveChanges();


            return RedirectToAction("Edit");
        }

        public ActionResult UpdateCandidateAvailableDate(DateTime availableDate)
        {
            var user = GetUser();
            var candidate = GetCandidateDetailsByUser(user.Id);
            //set jobtitleenum

            candidate.AvailableFrom = availableDate;
            
            //need to clear skills so no conlfict 
            candidate.Skills.Clear();
            //update and save changes 

            _db.Candidates.Update(candidate);
            _db.SaveChanges();


            return RedirectToAction("Edit");
        }

        public ActionResult ToggleCandidateAvailability()
        {
            var user = GetUser();
            var candidate = GetCandidateDetailsByUser(user.Id);
            if (candidate.IsAvailable)
            {
                candidate.IsAvailable = false;
            }
            else
            {
                candidate.IsAvailable = true; 
            }
            candidate.Skills.Clear();
            //update and save changes 

            _db.Candidates.Update(candidate);
            _db.SaveChanges();
            return RedirectToAction("Edit");
        }


        public List<CandidateProfile> GetAvailableCandidates()
        {

            
            var availableCandidates = (from c in _db.Candidates
                                       join u in _db.Users on
                                       c.UserId equals u.Id
                                       where c.IsAvailable == true
                                       select new CandidateProfile
                                       {
                                           FullName = u.FirstName+" "+u.LastName, 
                                           FirstName = u.FirstName,
                                           LastName = u.LastName,
                                           LevelEnum = c.LevelEnum,
                                           Rating = c.Rating,
                                           Rate = c.Rate,
                                           //Skill = c.Skill,
                                           CandidateID = c.CandidateID,
                                           Level = c.LevelEnum.GetDisplayName(), 
                                           JobTitle = c.JobTitleEnum.GetDisplayName(), 
                                           AvailableFrom = c.AvailableFrom, 
                                           Skills = c.Skills


                                       }).ToList();


            return availableCandidates; 
          
        }
        

    }



}
