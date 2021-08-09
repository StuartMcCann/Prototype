using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ApplicationUser FromUser { get; set; }
        public virtual ApplicationUser ToUser { get; set; }

        //Default Constructor 
        public ChatMessage()
        {

        }

        public ChatMessage(ApplicationUser fromUser, ApplicationUser toUser, string messageContent)
        {
            this.FromUser = fromUser;
            this.FromUserId = fromUser.Id;
            this.ToUser = toUser;
            this.ToUserId = toUser.Id;
            this.Message = messageContent;
            this.CreatedDate = DateTime.Now; 
        }
    }
   
}
