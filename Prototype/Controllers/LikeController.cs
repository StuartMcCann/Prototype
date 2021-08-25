using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            //creates a db objext for use in controller using dependency injection
            _db = db;
            _userManager = userManager;
        }


        public IActionResult Create( int candidateId)
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

        
        public IActionResult CreateLikeJob( int employerId, int jobId)
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



       
        public IActionResult CreateLikeEmployer( int employerId)
        {

            // ad validation here that not already liked?
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

        //TODO: GetLikesByEmployerId/ GetLikesByJobId/ GetLikesByCandidate/UserID
        public List<EmployerLike> GetLikesByEmployerId(int employerId)
        {

            List<EmployerLike> likes = (from l in _db.Likes
                                        join c in _db.Candidates
                                        on l.CandidateId equals c.CandidateID
                                        join u in _db.Users
                                        on c.UserId equals u.Id
                                        orderby l.DateLiked descending
                                        where l.EmployerId == employerId &&
           (l.LikeType == LikeType.CandidateLikesEmployer || l.LikeType == LikeType.CandidateLikesJob)
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



        public List<EmployerLike> GetLikesByCandidateId(int candidateId)
        {

            List<EmployerLike> likes = (from l in _db.Likes
                                        join e in _db.Employers on
                                        l.EmployerId equals e.EmployerId
                                        join u in _db.Users on
                                        e.EmployerId equals u.EmployerId
                                        orderby l.DateLiked descending
                                        where l.CandidateId == candidateId &&
                   (l.LikeType == LikeType.CandidateLikesEmployer || l.LikeType == LikeType.CandidateLikesJob)

                                        select new EmployerLike
                                        {
                                            CandidateId = l.CandidateId,
                                            EmployerId = l.EmployerId,
                                            LikeType = l.LikeType,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Employer = l.Employer


                                        }).ToList();

            return likes;

        }


        
        public IActionResult RemoveLikeCandidate( int candidateId)
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
       

      
        public ActionResult RemoveLikeForJob( int jobId)
        {
            
                var user = GetUser();
                var candidateId = GetCandidateIdByUserID(user.Id);
                var employerId = _db.Jobs.Where(j => j.JobId == jobId).First().EmployerRefId;
            var likeType = LikeType.CandidateLikesJob;
            //= new Like(likeType, employerId, candidateId,jobId, _db);
            var likeToRemove = _db.Likes.Where(l => l.LikeType == likeType && l.CandidateId == candidateId && l.JobId == jobId).First();
            if (ModelState.IsValid)
            {
                _db.Likes.Remove(likeToRemove);

                _db.SaveChanges();
            }
            return RedirectToAction("JobProfile", "Job", new { id = jobId }); 
            
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

            var likes = LikeHelper.GetLikesForMutualLikeCheck(_db, employerId, candidateId); 
            //loop here to check result has right like types 
            if (likes != null)
            {
                mutualLike = LikeHelper.LikeTypeCount(likes);
            }

            return mutualLike;
        }

   

        public bool AlreadyLiked(int id)
        {
            var alreadyLiked = false;
            var user = GetUser();
            if (User.IsInRole("Employer"))
            {
                //checks DB for likes in by by candidate to employer
                alreadyLiked = LikeHelper.CheckEmployerLikesCandidate(id, user, _db);

            }
            else if (User.IsInRole("Candidate"))
            {
                //checks DB for likes in by by Employer to Candidate
                alreadyLiked = LikeHelper.CheckCandidateLikesEmployer(id, user, _db);
            }

            return alreadyLiked;
        }

     

        public bool AlreadyLikedJob(int jobId)
        {
            
            var user = GetUser();
            var candidateId = GetCandidateIdByUserID(user.Id);
            var liked = LikeHelper.CheckAlreadyLikedJob(jobId, user, _db); 


            return liked;


        }

    }
}
