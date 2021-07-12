using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _db; 

        public CandidateController(ApplicationDbContext db)
        {
            //creates a db objext for use in controller using dependency injection
            _db = db;  
        }

        //can change to have level and skills - pass skills as a List?
        public ActionResult GetCandidatesLikeThis(string skill, int id)
        {
            var canidatesLikethis = (from c in _db.Candidates
                                where c.Skill == skill && c.CandidateID!=id
                                select new CandidateProfile
                                {
                                    Name = c.Name, 
                                    Level = c.Level, 
                                    Rating = c.Rating, 
                                    Rate = c.Rate, 
                                    Skill = c.Skill, 
                                    CandidateId = c.CandidateID, 
                                    LevelEnum = c.LevelEnum

                                }).ToList();

            return Json(new { data = canidatesLikethis });

        }

        public IActionResult Index()
        {
           // List<Candidate> list =  _db.Candidates.Where(s => s.Level == "Entry").OrderByDescending(c=>c.Rating).ToList<Candidate>(); 
            
            //below gets candidates from db
            IEnumerable<Candidate> candidateList = _db.Candidates;
            return View(candidateList);
        }

        public IActionResult CandidateProfile(int id)
        {

            var candidate = (from c in _db.Candidates
                                 //join r in _db.Reviews on c.CandidateId equals
                                 // r.CandidateRefId
                            where c.CandidateID == id
                             select new CandidateProfile
                             {
                                 CandidateId = c.CandidateID,
                                 Name = c.Name,
                                 Level = c.Level,
                                 Skill = c.Skill,
                                 Rating = c.Rating,
                                 Rate = c.Rate
                                 //for loop here to add to list of reviews or can do ajax call on page t print 

                             }).ToList();



            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }


    }



}
