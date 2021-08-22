using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service
{
    public class JobMatchHelper
    {

        public static List<CandidateProfile> JobMatchWithCandidates(ApplicationDbContext _db, Job job)
        {

            var finalMatches = new List<CandidateProfile>(); 
            var matchedCandidatesByJobtitle = MatchByJobtitle(_db, job.JobTitleEnum); 
            //if job title has no matches exit or check for skills?
            if(matchedCandidatesByJobtitle.Count() == 0)
            {
                // returns no matches by job title 
                return finalMatches;  
            }
            var jobSkills = GetJobSkills(_db, job); 

            var matchedCandidatesBySkillsAndJobTitle = CheckForMatchedSkills(matchedCandidatesByJobtitle, jobSkills); 

            if(matchedCandidatesBySkillsAndJobTitle.Count() == 0)
            {
                //no candidates with job title and skills so we return with job title match only 
                finalMatches = matchedCandidatesByJobtitle;
               
            }
            else
            {
                //matched with both  job title and skill so these are best match and will be returned 
                finalMatches = matchedCandidatesBySkillsAndJobTitle; 
            }

            //could add level here if required 
            return finalMatches; 

        }


        public static List<CandidateProfile> CheckForMatchedSkills(List<CandidateProfile> candidates, ICollection<Skill> jobSkills)
        {
            var candidatesMatchedBySkills = new List<CandidateProfile>();

            foreach(CandidateProfile candidate in candidates)
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
        public static List<Skill> GetJobSkills(ApplicationDbContext _db , Job job)
        {
            var jobSkills = new List<Skill>();

            foreach (int skillId in job.SkillIds)
            {
                jobSkills.Add(_db.Skills.Where(s => s.SkillId == skillId).First());
            }

            return jobSkills; 
        }
        

        public static List<CandidateProfile> MatchByJobtitle(ApplicationDbContext _db, JobTitle jobTitle)
        {

            //get match for job title
            var similarJobTitles = GetSimilarJobTitles(jobTitle);

            var matchedCandidatesByJobtitle = (from c in _db.Candidates
                                               where (c.JobTitleEnum == jobTitle
                                               || similarJobTitles.Contains(c.JobTitleEnum)) &&c.IsAvailable
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

        

        public static List<JobTitle> GetSimilarJobTitles(JobTitle jobTitle)
        {
            var similarJobTitles = new List<JobTitle>(); 
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


        public static List<JobProfile> GetJobsStartingSoon(JobTitle jobTitle, ApplicationDbContext _db)
        {
            var similarJobTitles = GetSimilarJobTitles(jobTitle);


            // below gets date 1 week in furtue to use in query 
            var dateCriteria = DateTime.Now.AddDays(+7);

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
                                        DisplayStartDate = j.StartDate.ToShortDateString()

                                    }).ToList(); 




            return jobsStartingSoon; 



        }





    }
}
