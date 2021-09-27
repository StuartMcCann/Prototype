using Microsoft.EntityFrameworkCore;
using Prototype.Data;
using Prototype.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Service
{
    public class ChatHelper
    {
        // get distinct conversations for a user 
        public static List<DisplayMessage> GetConversations(ApplicationDbContext _db, string userId)
        {
            return _db.ChatMessages.Where(m => m.ToUserId == userId || m.FromUserId == userId)
                .Select(c => new DisplayMessage
                {
                    //conditional statements to check which user is reciever and sender for display purposes 
                    FullName = (c.ToUserId == userId) ? c.FromUser.FirstName + " " + c.FromUser.LastName : c.ToUser.FirstName + " " + c.ToUser.LastName,
                    DisplayId = (c.ToUserId == userId) ? c.FromUser.Id : c.ToUser.Id
                }).Distinct().ToList();

        }
        //get all chat messages between 2 users 
        public static List<ChatMessage> GetChatHistory(ApplicationDbContext _db, string userId, string toUserId)
        {
            return _db.ChatMessages
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
        }

        //get user information for candidate 
        public static UserProfile GetUserProfileCandidate(ApplicationDbContext _db, string userId)
        {
            return (from u in _db.Users

                    join c in _db.Candidates
                    on u.Id equals c.UserId
                    where u.Id == userId
                    select new UserProfile
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        ProfilePicture = u.ProfilePicture,

                        Rating = c.Rating,
                        CandidateId = c.CandidateID,
                        Id = userId,
                    }).FirstOrDefault();

        }

        //get user information for employer 
        public static UserProfile GetUserProfileEmployer(ApplicationDbContext _db, string userId)
        {
            return (from u in _db.Users

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

        }
    }
}
