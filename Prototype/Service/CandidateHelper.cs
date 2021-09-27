using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Service
{
    public class CandidateHelper
    {
        //find candidates with matching job title
        public static List<CandidateProfile> GetCandidatesLikeThis(ApplicationDbContext _db, int candidateId)
        {
            //value to limit number of candidates 
            var numberOfCandidates = 3;
            //get candidate by id 
            var candidate = (from c in _db.Candidates
                             where c.CandidateID == candidateId
                             select new Candidate
                             {
                                 CandidateID = c.CandidateID,
                                 Skills = c.Skills,
                                 JobTitleEnum = c.JobTitleEnum
                             }).FirstOrDefault();

            //get candidates with matching job title 
            var candidatesLikeThis = (from c in _db.Candidates
                                      join u in _db.Users on
                                      c.UserId equals u.Id
                                      where c.CandidateID != candidateId
                                      && c.IsAvailable == true && c.JobTitleEnum == candidate.JobTitleEnum
                                      select new CandidateProfile
                                      {

                                          FirstName = u.FirstName,
                                          LastName = u.LastName,
                                          LevelEnum = c.LevelEnum,
                                          Rating = c.Rating,
                                          Rate = c.Rate,
                                          JobTitleEnum = c.JobTitleEnum,
                                          CandidateID = c.CandidateID,
                                          Level = c.LevelEnum.GetDisplayName(),
                                          Skills = c.Skills


                                      }).Take(numberOfCandidates).ToList();
          
            return candidatesLikeThis;



        }
        //get candidate information by candidate id 
        public static CandidateProfile GetCandidateProfile(ApplicationDbContext _db, int candidateId)
        {
            return ((from c in _db.Candidates
                     join u in _db.Users
                     on c.UserId equals u.Id
                     where c.CandidateID == candidateId
                     select new CandidateProfile
                     {
                         CandidateID = c.CandidateID,
                         LevelEnum = c.LevelEnum,
                         Rating = c.Rating,
                         Rate = c.Rate,
                         UserId = u.Id,
                         ProfilePicture = u.ProfilePicture,
                         FirstName = u.FirstName,
                         LastName = u.LastName,
                         Skills = c.Skills,
                         Likes = c.Likes,
                         Contracts = c.Contracts,
                         AvailableFrom = c.AvailableFrom
                     })).FirstOrDefault();
        }

        public static CandidateProfile GetCandidateDetailsByUser(ApplicationDbContext _db, string userId)
        {
            return (from c in _db.Candidates
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
        }
        //get all available candidates in db 
        public static List<CandidateProfile> GetAvailableCandidates(ApplicationDbContext _db)
        {
            return (from c in _db.Candidates
                    join u in _db.Users on
                    c.UserId equals u.Id
                    where c.IsAvailable == true
                    select new CandidateProfile
                    {
                        FullName = u.FirstName + " " + u.LastName,
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
        }




    }
}
