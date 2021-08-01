﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Prototype.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public ChatController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _db = applicationDbContext;
        }

        //public IActionResult Index()
        //{
        //    var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
        //    IEnumerable<ApplicationUser> users = _db.Users.Where(user => user.Id != userId).ToList();

        //    return View(users);
        //}

        //[HttpGet("users")]
        public List<ApplicationUser> GetUsers()
        {

            //add query here to get only those who can message 
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers = _db.Users.Where(user => user.Id != userId).ToList();
            return allUsers;
        }

        //change this to messahe user/ chane message user page to index and this to index
        public IActionResult Index(string userId)
        {
            var user = GetUserDetails(userId);
            UserProfile userProfile;

            if (user.EmployerId == null)
            {
                userProfile = GetUserProfileCandidate(userId);
                return View(userProfile);
            }
            else
            {
                userProfile = GetUserProfileEmployer(userId);
                return View(userProfile);
            }


           


        }



        //[HttpGet("users/{userId}")]
        public ApplicationUser GetUserDetails(string userId)
        {
            var user = _db.Users.Where(user => user.Id == userId).FirstOrDefault();
            return user;
        }

        public UserProfile GetUserProfileCandidate(string userId)
        {
            UserProfile userProfile = (from u in _db.Users

                                       join c in _db.Candidates
                                       on u.Id equals c.UserId
                                       where u.Id == userId
                                       select new UserProfile
                                       {
                                           FirstName = u.FirstName,
                                           LastName = u.LastName,
                                           ProfilePicture = u.ProfilePicture,
                                           Skill = c.Skill,
                                           Rating = c.Rating,
                                           CandidateId = c.CandidateID, 
                                           Id = userId,
                                       }).FirstOrDefault();




            return userProfile;
        }


        public UserProfile GetUserProfileEmployer(string userId)
        {
            UserProfile userProfile = (from u in _db.Users

                                       join e in _db.Employers
                                       on u.EmployerId equals e.EmployerId
                                       where u.Id == userId
                                       select new UserProfile
                                       {
                                           FirstName = u.FirstName,
                                           LastName = u.LastName,
                                           ProfilePicture = u.ProfilePicture,
                                           CompanyName = e.CompanyName,
                                           Rating = e.Rating,
                                           Id = userId
                                       }).FirstOrDefault();




            return userProfile;
        }



        [HttpPost]
        public ActionResult SaveMessage(string toUserId, string messageContent)
        {
            
            ChatMessage message = new ChatMessage();
            //separate methods for belwp
            var userId = GetCurrentUserID();
            var fromUser = GetUserByUserId(userId); 
            var toUser = GetUserByUserId(toUserId);

            message.ToUser = toUser;
            message.FromUser = fromUser;

            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;


            message.Message = messageContent;
            message.ToUserId = toUserId;

            _db.ChatMessages.Add(message);
            _db.SaveChanges();
            return RedirectToAction("Index", new { userId = toUserId });

            
           
        }

        public string GetCurrentUserID()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            return userId; 
        }

        public ApplicationUser GetUserByUserId(string userId)
        {
           var user =  _db.Users.Where(user => user.Id == userId).FirstOrDefault();
            return user; 
        }

        //need to pass id for cand/client here 
        public List<ChatMessage> GetConversation(string toUserId)
        {
            var userId = GetCurrentUserID(); 
            var messages = _db.ChatMessages
                    .Where(h => (h.FromUserId == toUserId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == toUserId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatMessage
                    {
                        FromUserId = x.FromUserId,
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUser = x.ToUser,
                        FromUser = x.FromUser
                    }).ToList();


            return messages;
        }

    }
}
