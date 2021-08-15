using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Enums;
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

       

        

        // POST: LikeController/Create for employer likes candidate
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult Create(LikeType likeType,  int candId )
        {

            
            var user = GetUser();
            int employerId = (int)user.EmployerId;

            
                Like like = new Like(likeType, employerId, candId, _db);
             
            

            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();
                return RedirectToAction("CandidateProfile", "Candidate", new { id = like.CandidateId });
               

            }

            return NotFound(); 




        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult CreateLikeJob(LikeType likeType, int employerId, int jobId)
        {


            var user = GetUser();
            String userId = user.Id;
            var candId = GetCandidateIdByUserID(userId); 
            

            
                Like like = new Like(likeType, employerId, candId, jobId, _db); 
          
            



            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();
                return RedirectToAction("JobProfile", "Job", new { id = like.JobId });

            }

            return NotFound(); 



        }

       

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult CreateLikeEmployer(LikeType likeType, int employerId)
        {

            // ad validation here that not already liked?

            var user = GetUser();
            String userId = user.Id;
            var candId = GetCandidateIdByUserID(userId);

            Like like = new Like(likeType, employerId, candId, _db);


            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();
                //page refresg
                return RedirectToAction("Index", "Employer", new { id = like.EmployerId });
               

            }

            return NotFound();

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
        public void RemoveLikeCandidate(LikeType likeType, int candidateId)
        {
            try
            {
                var user = GetUser();
                var employerId = (int)user.EmployerId; 
                var LikeToRemove = new Like(likeType, employerId, candidateId, _db);

                _db.Likes.Remove(LikeToRemove);

                _db.SaveChanges(); 


            }
            catch
            {
               
            }
        }






        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user;
        }

        public int GetCandidateIdByUserID(String userId)
        {
             
            
            var candidateId = _db.Candidates.Where(c => c.UserId == userId).Select(i => i.CandidateID).First();
           
                
           
           

            return candidateId;
        }

        public bool HasMutualLike(int id)
        {
            var mutualLike = false;
            var user = GetUser();
            var candidateId = 0;
            var employerId = 0;
            if (User.IsInRole("Employer"))
            {
                employerId = (int)user.EmployerId;
                candidateId = id; 

            }
            else if (User.IsInRole("Candidate"))
            {
                employerId = id;
                candidateId = GetCandidateIdByUserID(user.Id); 
               
            }

            
            var likes = (from l in _db.Likes
                         where
    (l.EmployerId == employerId && l.CandidateId == candidateId && l.LikeType == LikeType.EmployerLikesCandidate)
    || ((l.EmployerId == employerId && l.CandidateId == candidateId && l.LikeType == LikeType.CandidateLikesEmployer)
    || (l.EmployerId == employerId && l.CandidateId ==candidateId && l.LikeType == LikeType.CandidateLikesJob))
                         select new Like
                         {
                             LikeType = l.LikeType,
                             JobId = l.JobId,
                             EmployerId = l.EmployerId,
                             CandidateId = l.CandidateId


                         }).ToList();
            //loop here to check result has right like types 
            if (likes != null)
            {
                mutualLike = LikeTypeCount(likes);
            }
           
            return mutualLike; 
        }

        public bool LikeTypeCount(List<Like> likes)
        {
            var mutual = false;
            var employerCount = 0;
            var candidateCount = 0; 
            //loop through to check like types 
            foreach(Like like in likes)
            {
                employerCount = like.LikeType == LikeType.EmployerLikesCandidate ? ++employerCount : employerCount;
                candidateCount = like.LikeType == LikeType.CandidateLikesEmployer || like.LikeType == LikeType.CandidateLikesJob ?
                    ++candidateCount : candidateCount;
                if (employerCount > 0 && candidateCount > 0)
                {
                    mutual = true;
                    return mutual; 
                }

            }

            return mutual; 
        }

        public bool AlreadyLiked(int id)
        {
            var alreadyLiked = false;

            if (User.IsInRole("Employer"))
            {
                //checks DB for likes in by by candidate to employer
                alreadyLiked = CheckEmployerLikesCandidate(id);
                 
            }else if (User.IsInRole("Candidate"))
            {
                //checks DB for likes in by by Employer to Candidate
                alreadyLiked = CheckCandidateLikesEmployer(id);
            }

            return alreadyLiked; 
        }

        //checks DB for likes in by by candidate to employer
        public bool CheckCandidateLikesEmployer(int employerId)
        {
            var liked = false;
            var user = GetUser();
            var candidateId = GetCandidateIdByUserID(user.Id);
            var likes = (from l in _db.Likes
                         where l.EmployerId == employerId
                         && l.CandidateId == candidateId
                         && (l.LikeType == LikeType.CandidateLikesEmployer || l.LikeType == LikeType.CandidateLikesJob)
                         select new Like
                         {
                             LikeType = l.LikeType,
                             JobId = l.JobId,
                             EmployerId = l.EmployerId,
                             CandidateId = l.CandidateId


                         }).ToList();
            if (likes.Count() > 0)
            {
                liked = true;
            }


            return liked;

          
        }

        public bool CheckEmployerLikesCandidate(int candidateId)
        {
            var liked = false;
            var user = GetUser();
            var likes = (from l in _db.Likes
                         where l.CandidateId == candidateId
                         && l.EmployerId == user.EmployerId
                         && (l.LikeType == LikeType.EmployerLikesCandidate)
                         select new Like
                         {
                             LikeType = l.LikeType,
                             JobId = l.JobId,
                             EmployerId = l.EmployerId,
                             CandidateId = l.CandidateId


                         }).ToList();
            if (likes.Count() > 0)
            {
                liked = true;
            }


            return liked;

        }

        public bool AlreadyLikedJob(int jobId)
        {
            var liked = false;
            var user = GetUser();
            var candidateId = GetCandidateIdByUserID(user.Id);
            var likes = (from l in _db.Likes
                         where l.JobId == jobId
                         && l.CandidateId == candidateId
                         && ( l.LikeType == LikeType.CandidateLikesJob)
                         select new Like
                         {
                             LikeType = l.LikeType,
                             JobId = l.JobId,
                             EmployerId = l.EmployerId,
                             CandidateId = l.CandidateId


                         }).ToList();
            if (likes.Count() > 0)
            {
                liked = true;
            }


            return liked;


        }

    }
}
