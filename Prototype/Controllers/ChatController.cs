using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Prototype.Data;
using Prototype.Models;
using Prototype.Service;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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

        #region CrudAndPageNav       
        [Authorize]
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

        [HttpPost]
        public ActionResult SaveMessage(string toUserId, string messageContent)
        {
            var userId = GetCurrentUserID();
            var fromUser = GetUserByUserId(userId);
            var toUser = GetUserByUserId(toUserId);

            ChatMessage message = new ChatMessage(fromUser, toUser, messageContent);

            if (ModelState.IsValid)
            {
                _db.ChatMessages.Add(message);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", new { userId = toUserId });

        }

        #endregion

        #region GetMethods
        public ApplicationUser GetUserDetails(string userId)
        {
            var user = _db.Users.Where(user => user.Id == userId).FirstOrDefault();
            return user;
        }

        public UserProfile GetUserProfileCandidate(string userId)
        {
            UserProfile userProfile = ChatHelper.GetUserProfileCandidate(_db, userId);
            return userProfile;
        }


        public UserProfile GetUserProfileEmployer(string userId)
        {
            UserProfile userProfile = ChatHelper.GetUserProfileEmployer(_db, userId);

            return userProfile;
        }

        public IActionResult EmployerPageRedirect(int employerId)
        {
            var userId = _db.Users.Where(u => u.EmployerId == employerId).Select(i => i.Id).First();

            return RedirectToAction("Index", new { userId = userId });

        }
        public string GetCurrentUserID()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            return userId;
        }

        public ApplicationUser GetUserByUserId(string userId)
        {
            var user = _db.Users.Where(user => user.Id == userId).FirstOrDefault();
            return user;
        }

        
        public List<ChatMessage> GetChatHistory(string toUserId)
        {
            var userId = GetCurrentUserID();
            var messages = ChatHelper.GetChatHistory(_db, userId, toUserId);
            return messages;
        }

        public ActionResult GetConversations()
        {
            var userId = GetCurrentUserID();
            var messages = ChatHelper.GetConversations(_db, userId);
            return Json(messages);
        }


        public List<ApplicationUser> GetUsers()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers = _db.Users.Where(user => user.Id != userId).ToList();
            return allUsers;
        }
        #endregion
    }
}
