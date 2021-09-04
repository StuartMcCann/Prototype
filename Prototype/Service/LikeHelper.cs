﻿using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Service
{
    public class LikeHelper
    {

        public static List<EmployerLike> GetLikesByEmployerId(ApplicationDbContext _db, int employerId)
        {
            return (from l in _db.Likes
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

        }

        public static List<EmployerLike> GetLikesByJobId(ApplicationDbContext _db, int jobId)
        {
            return (from l in _db.Likes
                    join c in _db.Candidates
                    on l.CandidateId equals c.CandidateID
                    join u in _db.Users
                    on c.UserId equals u.Id
                    orderby l.DateLiked descending
                    where l.JobId == jobId &&
                    (l.LikeType == LikeType.CandidateLikesEmployer || l.LikeType == LikeType.CandidateLikesJob)
                    select new EmployerLike
                    {
                        CandidateId = c.CandidateID,
                        EmployerId = l.EmployerId,
                        LikeType = l.LikeType,
                        FirstName = u.FirstName,
                        LastName = u.LastName, 
                      CandidateJobTitle = c.JobTitleEnum.GetDisplayName(), 
                      DateLiked = l.DateLiked, 
                      Candidate =c



                    }).ToList();

        }

        public static List<EmployerLike> GetLikesByCandidateId(ApplicationDbContext _db, int candidateId)
        {
            return (from l in _db.Likes
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

        }


        public static bool CheckAlreadyLikedJob(int jobId, ApplicationUser user, ApplicationDbContext _db)
        {
            var candidateId = GetCandidateIdByUserID(user.Id, _db);
            var likes = (from l in _db.Likes
                         where l.JobId == jobId
                         && l.CandidateId == candidateId
                         && (l.LikeType == LikeType.CandidateLikesJob)
                         select new Like
                         {
                             LikeType = l.LikeType,
                             JobId = l.JobId,
                             EmployerId = l.EmployerId,
                             CandidateId = l.CandidateId


                         }).ToList();
            if (likes.Count() > 0)
            {
                return true;
            }


            return false;
        }


        public static bool CheckCandidateLikesEmployer(int employerId, ApplicationUser user, ApplicationDbContext _db)
        {
            var liked = false;

            var candidateId = GetCandidateIdByUserID(user.Id, _db);
            var likes = (from l in _db.Likes
                         where l.EmployerId == employerId
                         && l.CandidateId == candidateId
                         && (l.LikeType == LikeType.CandidateLikesEmployer /*|| l.LikeType == LikeType.CandidateLikesJob*/)
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

        public static bool CheckEmployerLikesCandidate(int candidateId, ApplicationUser user, ApplicationDbContext _db)
        {
            var liked = false;

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










        public static List<Like> GetLikesForMutualLikeCheck(ApplicationDbContext _db, int employerId, int candidateId)
        {
            return (from l in _db.Likes
                    where
(l.EmployerId == employerId && l.CandidateId == candidateId && l.LikeType == LikeType.EmployerLikesCandidate)
|| ((l.EmployerId == employerId && l.CandidateId == candidateId && l.LikeType == LikeType.CandidateLikesEmployer)
|| (l.EmployerId == employerId && l.CandidateId == candidateId && l.LikeType == LikeType.CandidateLikesJob))
                    select new Like
                    {
                        LikeType = l.LikeType,
                        JobId = l.JobId,
                        EmployerId = l.EmployerId,
                        CandidateId = l.CandidateId


                    }).ToList();
        }


        public static bool LikeTypeCount(List<Like> likes)
        {
            var mutual = false;
            var employerCount = 0;
            var candidateCount = 0;
            //loop through to check like types 
            foreach (Like like in likes)
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


        public static int GetCandidateIdByUserID(String userId, ApplicationDbContext _db)
        {
            var candidateId = _db.Candidates.Where(c => c.UserId == userId).Select(i => i.CandidateID).First();
            return candidateId;
        }


    }
}