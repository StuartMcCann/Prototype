using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Prototype.Controllers
{
    
    public class EmployerController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public EmployerController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
           _userManager = userManager;
            
        }


        //if employer profile is created sends to create if not 
        //public IActionResult Index()
        //{
        //    //get the application user details 
        //    var user = GetUser();
        //    var employerId = user.EmployerId; 
        //    Employer employer = _db.Employers.Find(employerId);
        //    if (employer == null)
        //    {
        //        return RedirectToAction("Create");
        //    }
        //    else
        //    {
        //        return View(employer);
        //    }


        //}

        public IActionResult Index(int id)
        {

            var employer = _db.Employers.Find(id); 
            
                return View(employer);
            


        }

        public IActionResult Hub()
        {

            var user = GetUser();
            var employer = GetEmployer(user.Id);
            if (employer != null)
            {
                return View(employer);
            }
            else
            {
                return RedirectToAction("Create"); 
            }
            
        }

        public Employer GetEmployer(String userID)
        {
            Employer employer = _db.Employers.Find(userID);
            
            return employer;
            

        }

        //get for create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employer employer)
        {
            //validation below 
            if (ModelState.IsValid)
            {
                //check for file / image and convert to byte array for storage in db
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        employer.CompanyLogo = dataStream.ToArray();
                    }
                   
                }
                //add employer created to DB
                _db.Employers.Add(employer);
                //save changes exexutes action to DB
                _db.SaveChanges();
               
                var  user = GetUser(); 
                user.EmployerId = employer.EmployerId;
                _db.SaveChanges(); 
                return RedirectToAction("Index");

            }
            return View(employer);
            
        }

        
        public IActionResult Edit()
        {
            //get the application user details 
            var user = GetUser();
            var employerId = user.EmployerId;
            Employer employer = _db.Employers.Find(employerId);
            if (employer == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                return View(employer);
            }
        }

        //post for edit 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(Employer employer)
        {

            if (ModelState.IsValid)
            {
                //converting image file to byte array for storage in db
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        employer.CompanyLogo = dataStream.ToArray();
                    }

                }
                _db.Employers.Update(employer);
                _db.SaveChanges();
                return View(employer);
            }
            else
            {
                return View(employer); 
            }

        }

        public IActionResult Delete()
        {
            return View();
        }

        public ApplicationUser GetUser()
        {
            var userId = _userManager.GetUserId(User);
            ApplicationUser user = _db.Users.Find(userId);
            return user; 
        }

       
        



    }
}
