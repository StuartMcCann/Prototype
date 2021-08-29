using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Service
{
    public class AnalyticsHelper
    {

        public static Dictionary<String, double> GetRateAnalysisForJobTitle(ApplicationDbContext _db, JobTitle jobTitle)
        {
            //initialised variables
            Dictionary<string, double> analysedData = new Dictionary<string, double>();
            var lowestEntry = 0.0;
            var highestEntry = 0.0;
            var averageEntry = 0.0;
            var lowestIntermediate = 0.0;
            var highestIntermediate = 0.0;
            var averageIntermediate = 0.0;
            var lowestExpert = 0.0;
            var highestExpert = 0.0;
            var averageExpert = 0.0;
            //getAllvalues for job title
            var allRatesForEntryJobtitle = _db.Candidates.Where(c => c.JobTitleEnum == jobTitle && c.LevelEnum == Level.Entry).Select(r => r.Rate).ToList();
            var allRatesForIntermediateJobtitle = _db.Candidates.Where(c => c.JobTitleEnum == jobTitle && c.LevelEnum == Level.Intermediate).Select(r => r.Rate).ToList();
            var allRatesForExpertJobtitle = _db.Candidates.Where(c => c.JobTitleEnum == jobTitle && c.LevelEnum == Level.Expert).Select(r => r.Rate).ToList();


            //find average/lowest/highest if data available
            if (allRatesForEntryJobtitle.Count() > 0)
            {
                lowestEntry = allRatesForEntryJobtitle.Min();
                highestEntry = allRatesForEntryJobtitle.Max();
                averageEntry = allRatesForEntryJobtitle.Average();

            }
            if (allRatesForIntermediateJobtitle.Count() > 0)
            {
                lowestIntermediate = allRatesForIntermediateJobtitle.Min();
                highestIntermediate = allRatesForIntermediateJobtitle.Max();
                averageIntermediate = allRatesForIntermediateJobtitle.Average();
            }
            if (allRatesForExpertJobtitle.Count() > 0)
            {
                lowestExpert = allRatesForExpertJobtitle.Min();
                highestExpert = allRatesForExpertJobtitle.Max();
                averageExpert = allRatesForExpertJobtitle.Average();
            }

            //add to dictionary
            analysedData.Add("LowestEntry", lowestEntry);
            analysedData.Add("AverageEntry", averageEntry);
            analysedData.Add("HighestEntry", highestEntry);
            analysedData.Add("LowestIntermediate", lowestIntermediate);
            analysedData.Add("AverageIntermediate", averageIntermediate);
            analysedData.Add("HighestIntermediate", highestIntermediate);
            analysedData.Add("LowestExpert", lowestExpert);
            analysedData.Add("AverageExpert", averageExpert);
            analysedData.Add("HighestExpert", highestExpert);

            

            return analysedData;

        }


        public static Dictionary<string, double> InDemandSkills(ApplicationDbContext _db)
        {
            //get all skills and add to dictionary data structure 
            var skillsMapped = CountJobSkills(_db);
            // find top 4
            var topSkills = skillsMapped.OrderByDescending(p => p.Value).Take(4).ToDictionary(p=>p.Key, p=>p.Value);
            //find percentage of live jobs have the skills 
            var liveJobsCount = _db.Jobs.Where(j => j.IsLive == true).ToList().Count(); 
            var dataAnalysed = new Dictionary<string, double>(); 
            foreach(var item in topSkills)
            {
                double percentage = (((double)item.Value / liveJobsCount) * 100);
                dataAnalysed.Add(item.Key.SkillName, percentage); 
            }

            return dataAnalysed ; 


        }

        public static Dictionary<Skill, int> CountJobSkills(ApplicationDbContext _db)
        {

            var skillsMapped = new Dictionary<Skill, int>();
            var allJobs = (from j in _db.Jobs
                           where j.IsLive == true
                           select new Job
                           {
                               Skills = j.Skills
                           })
                            .ToList();
            //get count for each 

            foreach (Job job in allJobs)
            {
                foreach (Skill skill in job.Skills)
                {
                    if (!skillsMapped.ContainsKey(skill))
                    {
                        skillsMapped.Add(skill, 1);
                    }
                    else
                    {
                        skillsMapped[skill]++;
                    }
                }
            }

            return skillsMapped; 


        }


        public static Dictionary<string, double> PopularSkills(ApplicationDbContext _db)
        {
            //get all skills and add to dictionary data structure 
            var skillsMapped = CountCandidateSkills(_db);
            // find top 4
            var topSkills = skillsMapped.OrderByDescending(p => p.Value).Take(4).ToDictionary(p => p.Key, p => p.Value);
            //find percentage of live jobs have the skills 
            var canddidatesCount = _db.Candidates.ToList().Count();
            var dataAnalysed = new Dictionary<string, double>();
            foreach (var item in topSkills)
            {
                double percentage = (((double)item.Value / canddidatesCount) * 100);
                dataAnalysed.Add(item.Key.SkillName, percentage);
            }

            return dataAnalysed;


        }

        public static Dictionary<Skill, int> CountCandidateSkills(ApplicationDbContext _db)
        {

            var skillsMapped = new Dictionary<Skill, int>();
            var allCandidates = (from c in _db.Candidates
                           
                           select new Candidate
                           {
                               Skills = c.Skills
                           })
                            .ToList();
            //get count for each 

            foreach (Candidate candidate in allCandidates)
            {
                foreach (Skill skill in candidate.Skills)
                {
                    if (!skillsMapped.ContainsKey(skill))
                    {
                        skillsMapped.Add(skill, 1);
                    }
                    else
                    {
                        skillsMapped[skill]++;
                    }
                }
            }

            return skillsMapped;


        }

        public static List<Employer> GetTopRatedEmployers(ApplicationDbContext _db)
        {
            var topRatedEmployers = _db.Employers.OrderByDescending(e => e.Rating).Take(5).ToList();

            return topRatedEmployers; 


        }

        public static Dictionary<string, int> GetJobsTitlesCount(ApplicationDbContext _db)
        {
            
             var jobTitleCounts = new Dictionary<string, int>();
            //get all jobs 
            var allJobs = _db.Jobs.Where(j => j.IsLive == true).ToList(); 
            //add job titles to dictionary or update count 
            foreach(Job job in allJobs)
            {
                if (!jobTitleCounts.ContainsKey(job.JobTitleEnum.ToString())){
                    jobTitleCounts.Add(job.JobTitleEnum.ToString(), 1);
                }
                else
                {
                    jobTitleCounts[job.JobTitleEnum.ToString()]++; 
                }
            }

            return jobTitleCounts; 

        }

        public static Dictionary<string, int> GetBespokeCandidateData(ApplicationDbContext _db, int candidateId)
        {
            var analysedData = new Dictionary<string, int>();
            //get candidate 
            //var candidate = _db.Candidates.Where(c => c.CandidateID == candidateId).First();
            var candidate = (from c in _db.Candidates
                             where c.CandidateID == candidateId
                             select new Candidate
                             {
                                 JobTitleEnum = c.JobTitleEnum,
                                 Skills = c.Skills

                             }).FirstOrDefault(); 
            //get likes
            var likesPerCandidate = _db.Likes.Where(l => l.CandidateId == candidateId).ToList();

            //get numlikes
            var likesPerCandidateCount = likesPerCandidate.Count();

            //get num employers liked
            var numEmployerLikes = likesPerCandidate.GroupBy(e => e.EmployerId).Distinct().ToList().Count();
            
            //get num jobs with job title
            var numJobsWithJobTitle = _db.Jobs.Where(j => j.JobTitleEnum == candidate.JobTitleEnum).ToList().Count();
            //get % jobs with skills
            var percentage = GetPercentageOfJobsWithSkills(candidate, _db); 


            analysedData.Add("NumberOfLikes", likesPerCandidateCount);
            analysedData.Add("NumberOfEmployerLikes", numEmployerLikes);
            analysedData.Add("NumberOfJobsWithJobTitle", numJobsWithJobTitle);
            analysedData.Add("PercentageOfJobsWithSkillsMatch", percentage); 

            return analysedData; 
        }

        public static int GetPercentageOfJobsWithSkills(Candidate candidate, ApplicationDbContext _db)
        {
            var allJobs = (from j in _db.Jobs where j.IsLive == true select new Job { Skills = j.Skills }).ToList(); 
            var totalJobsCount = allJobs.Count();
            var skillsMatchCount = 0; 
            foreach(Job job in allJobs)
            {
                if (job.Skills.Intersect(candidate.Skills).Any())
                {
                    skillsMatchCount++; 
                }
            }

            var percentage = ((double)skillsMatchCount / totalJobsCount) * 100;

            return (int)percentage; 

        }

        public static Dictionary<string, int> GetJobsTitlesOfAvailableCandidates(ApplicationDbContext _db)
        {

            var jobTitleCounts = new Dictionary<string, int>();
            //get all jobs 
            var allCandidates = _db.Candidates.Where(c => c.IsAvailable == true).ToList();
            //add job titles to dictionary or update count 
            foreach (Candidate candidate in allCandidates)
            {
                if (!jobTitleCounts.ContainsKey(candidate.JobTitleEnum.ToString()))
                {
                    jobTitleCounts.Add(candidate.JobTitleEnum.ToString(), 1);
                }
                else
                {
                    jobTitleCounts[candidate.JobTitleEnum.ToString()]++;
                }
            }

            return jobTitleCounts;

        }

        public static Dictionary<string, int> GetBespokeEmployerData(ApplicationDbContext _db, int employerId)
        {
            var analysedData = new Dictionary<string, int>();
            //get employer
            var employer = (from e in _db.Employers
                            where e.EmployerId == employerId
                            select new Employer
                            {

                                Contracts = e.Contracts,
                                Jobs = e.Jobs,
                                Likes = e.Likes

                            }).FirstOrDefault();
            //get likes 
            var likes = _db.Likes.Where(l => l.EmployerId == employerId).ToList();
            var totalLikesCount = likes.Count();
            //get number of candidates who have likes 
            var numCandidateLikes = likes.GroupBy(e => e.CandidateId).Distinct().ToList().Count();
            //get % of jobs filled / success rate 
            var fillRate = PercentageFillRate(employer.Contracts, employer.Jobs);
            //get number of candidates from jobs that they have live 
            var numberOfCandidatesMatchJobs = GetNumberOfJobsWithMatch(_db, employer.Jobs); 
            //add to dictionary 
            analysedData.Add("TotalLikes", totalLikesCount);
            analysedData.Add("NumCandidateLikes", numCandidateLikes);
            analysedData.Add("FillRate",(int) fillRate);
            analysedData.Add("CandidatesMatched", numberOfCandidatesMatchJobs); 

            return analysedData;
        }

        public static double PercentageFillRate( ICollection<Contract> contracts, ICollection<Job> jobs)
        {
            double fillRate = 0;

            //get total Jobs by employer
            var totalJobs = jobs.Count();

            //Get filled jobs 
            var filledJobs = contracts.Count();

            //get percentage 
            fillRate = ((double)filledJobs / totalJobs) * 100; 


            return fillRate; 
        }

        public static int GetNumberOfJobsWithMatch(ApplicationDbContext _db, ICollection<Job> jobs)
        {
            int count = 0;
            //get candidates available
            var availableCandidates = _db.Candidates.Where(c => c.IsAvailable).ToList();
            //get all job titles from jobs 
                       
            var allJobTitles = new List<JobTitle>(); 
            
            foreach (Job job in jobs){
                if (!allJobTitles.Contains(job.JobTitleEnum)&& job.IsLive)
                {
                    allJobTitles.Add(job.JobTitleEnum); 
                }

            }


            foreach(Candidate candidate in availableCandidates)
            {
                if(allJobTitles.Contains(candidate.JobTitleEnum))
                {
                    count++; 
                }
            }


            return count;

            
        }




    }
}
