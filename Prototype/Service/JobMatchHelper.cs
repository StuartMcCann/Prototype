using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Service
{
    public class JobMatchHelper
    {
        //time frame for JobsStartingSoon method 
        protected const int TimeFrameInDays = 7; 

        //gets candodates which match job 
        public static List<CandidateProfile> JobMatchWithCandidates(ApplicationDbContext _db, Job job)
        {

            var finalMatches = new List<CandidateProfile>();
            //find candidates which match job title 
            var matchedCandidatesByJobtitle = MatchByJobtitle(_db, job.JobTitleEnum);
            //if job title has no matches exit - matches are ususaly guaranteed by Software Engineer job title matching all of job titles 
            if (matchedCandidatesByJobtitle.Count() == 0)
            {
                // returns no matches by job title 
                return finalMatches;
            }

            var jobSkills = GetJobSkills(_db, job);
            //check candidates matched by job title for matching skills i.e an improved match
            var matchedCandidatesBySkillsAndJobTitle = CheckForMatchedSkills(matchedCandidatesByJobtitle, jobSkills);

            if (matchedCandidatesBySkillsAndJobTitle.Count() == 0)
            {
                //no candidates with job title and skills so we return with job title match only 
                finalMatches = matchedCandidatesByJobtitle;

            }
            else
            {
                //matched with both  job title and skill so these are best match and will be returned 
                finalMatches = matchedCandidatesBySkillsAndJobTitle;
            }


            return finalMatches;

        }


        public static List<CandidateProfile> CheckForMatchedSkills(List<CandidateProfile> candidates, ICollection<Skill> jobSkills)
        {
            var candidatesMatchedBySkills = new List<CandidateProfile>();
            //loop through list of candidates that have been matched by job title
            foreach (CandidateProfile candidate in candidates)
            {
                //if intersect returns a result then there is a match with skills 
                if (candidate.Skills.Intersect(jobSkills).Any())
                {
                    candidatesMatchedBySkills.Add(candidate);
                }
            }
            return candidatesMatchedBySkills;
        }

        //required as skills in job need repopulated 
        public static List<Skill> GetJobSkills(ApplicationDbContext _db, Job job)
        {
            var jobSkills = new List<Skill>();

            foreach (int skillId in job.SkillIds)
            {
                jobSkills.Add(_db.Skills.Where(s => s.SkillId == skillId).First());
            }

            return jobSkills;
        }

        //returns candidates with similar or matching job title  
        public static List<CandidateProfile> MatchByJobtitle(ApplicationDbContext _db, JobTitle jobTitle)
        {

            //get list of similar job titles for db query 
            var similarJobTitles = GetSimilarJobTitles(jobTitle);
            //get candidates with same job title or similar job title 
            var matchedCandidatesByJobtitle = (from c in _db.Candidates
                                               where (c.JobTitleEnum == jobTitle
                                               || similarJobTitles.Contains(c.JobTitleEnum)) && c.IsAvailable
                                               select new CandidateProfile
                                               {
                                                   CandidateID = c.CandidateID,
                                                   ApplicationUser = c.ApplicationUser,
                                                   LevelEnum = c.LevelEnum,
                                                   Rating = c.Rating,
                                                   JobTitleEnum = c.JobTitleEnum,
                                                   Rate = c.Rate,
                                                   AvailableFrom = c.AvailableFrom,
                                                   Skills = c.Skills,
                                                   Likes = c.Likes,
                                                   Contracts = c.Contracts


                                               }).ToList();
            return matchedCandidatesByJobtitle;

        }


        //analysed job title to create list of similar job titles 
        public static List<JobTitle> GetSimilarJobTitles(JobTitle jobTitle)
        {
            var similarJobTitles = new List<JobTitle>();
            //switch / case statement to find job similr job titles and add to list 
            //If extending add SoftwareEngineer to each case to improve matches 
            switch (jobTitle)
            {
                case JobTitle.SoftwareEngineer:
                    similarJobTitles.Add(JobTitle.FullStackDeveloper);
                    similarJobTitles.Add(JobTitle.BackEndEngineer);
                    similarJobTitles.Add(JobTitle.SoftwareArchitect);
                    break;
                case JobTitle.WebDeveloper:
                    similarJobTitles.Add(JobTitle.FrontEndEngineer);
                    similarJobTitles.Add(JobTitle.FullStackDeveloper);
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    break;
                case JobTitle.DataEngineer:
                    similarJobTitles.Add(JobTitle.BackEndEngineer);
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.FullStackDeveloper);
                    break;
                case JobTitle.DevOpsEngineer:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    break;
                case JobTitle.SoftwareArchitect:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.TechnicalLead);
                    similarJobTitles.Add(JobTitle.FullStackDeveloper);
                    similarJobTitles.Add(JobTitle.BackEndEngineer);
                    break;
                case JobTitle.TechnicalLead:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.ProjectManager);
                    similarJobTitles.Add(JobTitle.SoftwareArchitect);
                    break;
                case JobTitle.ProjectManager:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.TechnicalLead);
                    break;
                case JobTitle.SoftwareTester:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    break;
                case JobTitle.FullStackDeveloper:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.WebDeveloper);
                    similarJobTitles.Add(JobTitle.DataEngineer);
                    similarJobTitles.Add(JobTitle.SoftwareArchitect);
                    similarJobTitles.Add(JobTitle.FrontEndEngineer);
                    similarJobTitles.Add(JobTitle.BackEndEngineer);
                    break;
                case JobTitle.FrontEndEngineer:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.WebDeveloper);
                    similarJobTitles.Add(JobTitle.FullStackDeveloper);
                    break;
                case JobTitle.BackEndEngineer:
                    similarJobTitles.Add(JobTitle.SoftwareEngineer);
                    similarJobTitles.Add(JobTitle.FullStackDeveloper);
                    similarJobTitles.Add(JobTitle.DataEngineer);
                    similarJobTitles.Add(JobTitle.SoftwareArchitect);
                    break;
                default:
                    break;

            }

            return similarJobTitles;

        }

        //gets jobs starting within time frame in days declared as constant 
        public static List<JobProfile> GetJobsStartingSoon(JobTitle jobTitle, ApplicationDbContext _db)
        {            
            //get similar job titles 
            var similarJobTitles = GetSimilarJobTitles(jobTitle);
            // below gets date in furtue to use in query 
            var dateCriteria = DateTime.Now.AddDays(+TimeFrameInDays);
            //gets jobs that are a match starting within specified timeframe 
            var jobsStartingSoon = (from j in _db.Jobs
                                    where (j.JobTitleEnum == jobTitle || similarJobTitles.Contains(jobTitle))
                                    && (j.StartDate <= dateCriteria && j.StartDate > DateTime.Now)
                                    orderby j.StartDate
                                    select new JobProfile
                                    {
                                        JobId = j.JobId,
                                        UpperRate = j.UpperRate,
                                        LowerRate = j.LowerRate,
                                        EmployerId = j.EmployerRefId,
                                        JobTitle = j.JobTitleEnum.GetDisplayName(),
                                        CompanyName = _db.Employers.Where(e => e.EmployerId == j.EmployerRefId).First().CompanyName,
                                        DisplayStartDate = j.StartDate.ToShortDateString(),
                                        StartDate = j.StartDate

                                    }).ToList();
            return jobsStartingSoon;
        }
    }
}
