using Microsoft.AspNetCore.Identity;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service
{
    public class JobHelper


    {


        
        public static List<JobProfile> GetUserJobs(ApplicationDbContext _db, int employerId)
        {

           return  (from j in _db.Jobs
             join u in _db.Users
             on j.EmployerRefId equals u.EmployerId
             where j.EmployerRefId == employerId
             && j.IsLive == true
             select new JobProfile
             {
                 JobId = j.JobId,
                 JobTitleEnum = j.JobTitleEnum,
                 StartDate = j.StartDate,
                 UpperRate = j.UpperRate,
                 LowerRate = j.LowerRate,
                 Duration = j.Duration,
                 JobDescription = j.JobDescription,
                 IsFilled = j.IsFilled,
                 IsLive = j.IsLive,
                 IsUnderContract = j.IsUnderContract,
                 EmployerRefId = j.EmployerRefId,
                 JobTitle = j.JobTitleEnum.GetDisplayName(),
                 Level = j.LevelEnum.GetDisplayName(),
                 Skills = j.Skills


             }).ToList();


        }

        public static List<JobProfile> GetJobsLikesThis(ApplicationDbContext _db, JobTitle jobTitle, int jobId)
        {
            var numberOfJobs = 3;

            var jobsLikeThis = (from j in _db.Jobs
                                where j.JobTitleEnum == jobTitle
                                && j.IsLive == true
                                && j.JobId != jobId
                                orderby j.StartDate
                                select new JobProfile
                                {
                                    JobId = j.JobId,

                                    JobDescription = j.JobDescription,
                                    UpperRate = j.UpperRate,
                                    LowerRate = j.LowerRate,
                                    JobTitleEnum = j.JobTitleEnum,
                                    CompanyName = j.Employer.CompanyName,
                                    Duration = j.Duration,
                                    StartDate = j.StartDate,
                                    Rating = j.Employer.Rating,
                                    Employer = j.Employer,
                                    JobTitle = j.JobTitleEnum.GetDisplayName(),
                                    Level = j.LevelEnum.GetDisplayName(),
                                    Skills = j.Skills,
                                    DisplayStartDate = j.StartDate.ToShortDateString()

                                }).Take(numberOfJobs).ToList();

            return jobsLikeThis;
        }

        public static JobProfile GetJobProfile(ApplicationDbContext  _db , int jobId)
        {
            return (from j in _db.Jobs
                    join employers in _db.Employers on j.EmployerRefId
                    equals employers.EmployerId
                    join u in _db.Users
                    on j.EmployerRefId equals u.EmployerId
                    //join jobTitle in _db.JobTitle on
                    //j.JobTitleRefId equals jobTitle.JobTitleId
                    where j.JobId == jobId
                    select new JobProfile
                    {
                        JobId = j.JobId,
                        JobDescription = j.JobDescription,
                        UpperRate = j.UpperRate,
                        LowerRate = j.LowerRate,
                        JobTitleEnum = j.JobTitleEnum,
                        CompanyName = employers.CompanyName,
                        Duration = j.Duration,
                        StartDate = j.StartDate,
                        Rating = employers.Rating,
                        EmployerId = employers.EmployerId,
                        UserId = u.Id,
                        Skills = j.Skills

                    }).FirstOrDefault();
        }

        public static List<JobProfile > GetAllLiveJobs(ApplicationDbContext _db)
        {

            return (from j in _db.Jobs
                    join e in _db.Employers on
                   j.EmployerRefId equals e.EmployerId
                    where j.IsLive == true
                    select new JobProfile
                    {
                        JobId = j.JobId,
                        JobTitle = j.JobTitleEnum.GetDisplayName(),
                        LowerRate = j.LowerRate,
                        UpperRate = j.UpperRate,
                        Duration = j.Duration,
                        JobDescription = j.JobDescription,
                        Level = j.LevelEnum.GetDisplayName(),
                        EmployerId = j.EmployerRefId,
                        //add skills required                                       
                        CompanyName = e.CompanyName,
                        Rating = e.Rating,
                        Skills = j.Skills

                    }).ToList();


        }



    }
}
