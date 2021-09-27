using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using Prototype.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Controllers
{
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LikeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            //creates a db object for use in controller using dependency injection
            _db = db;
            _userManager = userManager;
        }

        #region AddLikes
        //add like for employer liking candidate 
        public IActionResult Create(int candidateId)
        {
            var likeType = LikeType.EmployerLikesCandidate;

            var user = GetUser();
            int employerId = (int)user.EmployerId;
            Like like = new Like(likeType, employerId, candidateId, _db);

            if (ModelState.IsValid)
            {
                _db.Likes.Add(like);
                _db.SaveChanges();
                return RedirectToAction("CandidateProfile", "Candidate", new { id = like.CandidateId });
            }

            return NotFound();
        }

        //add like for candidate liking job 
        public IActionResult CreateLikeJob(int employerId, int jobId)
        {
            var likeType = LikeType.CandidateLikesJob;
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

        //create like for candidate liking employer 
        public IActionResult CreateLikeEmployer(int employerId)
        {
            var likeType = LikeType.CandidateLikesEmployer;
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
        #endregion


        #region RemoveLikes
        //remove like for employer likes candidate 
        public IActionResult RemoveLikeCandidate(int candidateId)
        {
            var likeType = LikeType.EmployerLikesCandidate;
            var user = GetUser();

            var likeToRemove = _db.Likes.Where(l => l.LikeType == likeType && l.CandidateId == candidateId && l.EmployerId == user.EmployerId).First();
            if (ModelState.IsValid)
            {
                _db.Likes.Remove(likeToRemove);
                _db.SaveChanges();
            }
            return RedirectToAction("CandidateProfile", "Candidate", new { id = candidateId });
        }
        //removve like for candidate likes employer 
        public IActionResult RemoveLikeForEmployer(int employerId)
        {
            var likeType = LikeType.CandidateLikesEmployer;
            var user = GetUser();
            var candidateId = GetCandidateIdByUserID(user.Id);
            var likeToRemove = _db.Likes.Where(l => l.LikeType == likeType && l.CandidateId == candidateId && l.EmployerId == employerId).First();
            if (ModelState.IsValid)
            {
                _db.Likes.Remove(likeToRemove);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Employer", new { id = employerId });
        }


        //remove like / unlike for candidate likes job
        public ActionResult RemoveLikeForJob(int jobId)
        {

            var user = GetUser();
            var candidateId = GetCandidateIdByUserID(user.Id);
            var employerId = _db.Jobs.Where(j => j.JobId == jobId).First().EmployerRefId;
            var likeType = LikeType.CandidateLikesJob;

            var likeToRemove = _db.Likes.Where(l => l.LikeType == likeType && l.CandidateId == candidateId && l.JobId == jobId).First();
            if (ModelState.IsValid)
            {
                _db.Likes.Remove(likeToRemove);

                _db.SaveChanges();
            }
            return RedirectToAction("JobProfile", "Job", new { id = jobId });

        }
        #endregion

        #region LikeChecks
        //checks for mutual like between candidates and employer 
        public bool HasMutualLike(int id)
        {
            var mutualLike = false;
            var user = GetUser();
            var candidateId = 0;
            var employerId = 0;
            //check user role 
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
            //get likes for candidate and employer 
            var likes = LikeHelper.GetLikesForMutualLikeCheck(_db, employerId, candidateId);
            //check result has right like types to be mutual like 
            if (likes != null)
            {
                mutualLike = LikeHelper.LikeTypeCount(likes);
            }
            return mutualLike;
        }


        //checks for an existing like between candidate and client 
        public bool AlreadyLiked(int id)
        {
            var alreadyLiked = false;
            var user = GetUser();
            //check user role 
            if (User.IsInRole("Employer"))
            {
                //checks DB for likes by candidate to employer
                alreadyLiked = LikeHelper.CheckEmployerLikesCandidate(id, user, _db);

            }
            else if (User.IsInRole("Candidate"))
            {
                //checks DB for likes by Employer to Candidate
                alreadyLiked = LikeHelper.CheckCandidateLikesEmployer(id, user, _db);
            }

            return alreadyLiked;
        }


        //check if a candidate has already liked a job 
        public bool AlreadyLikedJob(int jobId)
        {
            var user = GetUser();
            var candidateId = GetCandidateIdByUserID(user.Id);
            var liked = LikeHelper.CheckAlreadyLikedJob(jobId, user, _db);

            return liked;
        }
        #endregion

        #region GetMethods 
        public List<EmployerLike> GetLikesByEmployerId(int employerId)
        {
            List<EmployerLike> likes = LikeHelper.GetLikesByEmployerId(_db, employerId);

            return likes;

        }

        public IActionResult GetLikesByJobId(int jobId)
        {
            var jobLikes = LikeHelper.GetLikesByJobId(_db, jobId);

            return Json(jobLikes);
        }

        public List<EmployerLike> GetLikesByCandidateId(int candidateId)
        {

            List<EmployerLike> likes = LikeHelper.GetLikesByCandidateId(_db, candidateId);

            return likes;

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
        #endregion


    }
}
