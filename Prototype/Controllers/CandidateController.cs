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

        public IActionResult Index()
        {
           // List<Candidate> list =  _db.Candidates.Where(s => s.Level == "Entry").OrderByDescending(c=>c.Rating).ToList<Candidate>(); 
            
            //below gets candidates from db
            IEnumerable<Candidate> candidateList = _db.Candidates;
            return View(candidateList);
        }

       
    }
}
