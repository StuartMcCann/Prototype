using Microsoft.AspNetCore.Identity;
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

        public IActionResult Index()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            IEnumerable<ApplicationUser> users = _db.Users.Where(user => user.Id != userId).ToList();

            return View(users); 
        }

        //[HttpGet("users")]
        public  List<ApplicationUser> GetUsers()
        {

            //add query here to get only those who can message 
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers =  _db.Users.Where(user => user.Id != userId).ToList();
            return  allUsers;
        }

        


        //[HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            var user = await _db.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            return Ok(user);
        }



        [HttpPost]
        public async Task<IActionResult> SaveMessageAsync(ChatMessage message)
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;
            message.ToUser = await _db.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
            await _db.ChatMessages.AddAsync(message);
            return Ok(await _db.SaveChangesAsync());
        }

        //need to pass id for cand/client here 
        public  List<ChatMessage> GetConversation(string contactId)
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var messages = _db.ChatMessages
                    .Where(h => (h.FromUserId == contactId && h.ToUserId == userId) || (h.FromUserId == userId && h.ToUserId == contactId))
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

            //var messages = (from c in _db.ChatMessages
            //               where c.FromUserId == userId || c.ToUserId == userId
            //               select new ChatMessage
            //               {
            //                   Id = c.Id, 
            //                   FromUserId = c.FromUserId, 
            //                   ToUserId = c.ToUserId, 
            //                   Message = c.Message, 
            //                   CreatedDate = c.CreatedDate, 
            //                   ToUser = c.ToUser, 
            //                   FromUser = c.FromUser
            //               }).ToList(); 
            return messages;
        }

    }
}
