using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prototype.Data;
using Prototype.Enums;
using Prototype.Helpers;
using Prototype.Models;
using Prototype.Service;
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

        
        public ActionResult GetCandidatesLikeThis( int candidateId)
        {


            var candidatesLikeThis = CandidateHelper.GetCandidatesLikeThis(_db, candidateId); 

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

            //get the application user details 
            var user = GetUser();
            var userId = user.Id;
            var candidate = GetCandidateDetailsByUser(userId);
            //making sure a user has a candidate profile created 
            if (candidate != null)
            {
                return RedirectToAction("Edit");
            }
            else
            {
                //view bag for skills dropdowns 
                ViewBag.Skills = new SelectList(_db.Skills, "SkillId", "SkillName");
                return View();
            }
           
           
        }


        public IActionResult Index()
        {

            return View();
        }

        public IActionResult CandidateProfile(int id)
        {
            
            var candidate = CandidateHelper.GetCandidateProfile(_db, id); 
            

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
            var candidate = CandidateHelper.GetCandidateDetailsByUser(_db, userId); 

           

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


            var availableCandidates = CandidateHelper.GetAvailableCandidates(_db); 


            return availableCandidates; 
          
        }
        

    }



}
