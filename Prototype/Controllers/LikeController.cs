using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Controllers
{
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LikeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            //creates a db objext for use in controller using dependency injection
            _db = db;
            _userManager = userManager;
        }

       

        

        // POST: LikeController/Create
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public void Create(LikeType likeType,  int candId )
        {

            
            var user = GetUser();
            int employerId = (int)user.EmployerId;

            
                Like like = new Like(likeType, employerId, candId);
            
            

            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();
               
            }

           

        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public void CreateLikeJob(LikeType likeType, int employerId, int jobId)
        {


            var user = GetUser();
            String userId = user.Id;
            var candId = GetCandidateIdByUserID(userId); 
            

            
                Like like = new Like(likeType, employerId, candId, jobId); 
          
            



            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();

            }



        }

       

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public void CreateLikeEmployer(LikeType likeType, int employerId)
        {


            var user = GetUser();
            String userId = user.Id;
            var candId = GetCandidateIdByUserID(userId);

            Like like = new Like(likeType, employerId, candId);


            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();

            }



        }

        //TODO: GetLikesByEmployerId/ GetLikesByJobId/ GetLikesByCandidate/UserID
        public List<EmployerLike> GetLikesByEmployerId(int employerId)
        {

             List<EmployerLike> likes = (from l in _db.Likes
                                 join c in _db.Candidates
                                 on l.CandidateId equals c.CandidateID
                                 join u in _db.Users
                                 on c.UserId equals u.Id
                                 where l.EmployerId == employerId &&
            (l.LikeType == LikeType.CandidateLikesEmployer || l.LikeType ==  LikeType.CandidateLikesJob)
            select new EmployerLike
            {
                CandidateId = c.CandidateID, 
                EmployerId = l.EmployerId, 
                LikeType = l.LikeType, 
                FirstName = u.FirstName, 
                LastName = u.LastName

            }).ToList();

            return likes; 

        }



        // GET: LikeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LikeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LikeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LikeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }






        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user;
        }

        public int GetCandidateIdByUserID(String id)
        {
            var candidate = (from c in _db.Candidates
                             where c.UserId == id
                             select new Candidate
                             {
                                 Level = c.Level,
                                 Rating = c.Rating,
                                 Rate = c.Rate,
                                 Skill = c.Skill,
                                 CandidateID = c.CandidateID,
                                 LevelEnum = c.LevelEnum
                             }).FirstOrDefault();

            return candidate.CandidateID; 
        }

    }
}
