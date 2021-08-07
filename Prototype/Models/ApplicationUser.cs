﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototype.Models
{
    public class ApplicationUser : IdentityUser
    {


        public ApplicationUser()
        {
            ChatMessagesFromUsers = new HashSet<ChatMessage>();
            ChatMessagesToUsers = new HashSet<ChatMessage>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Profile Picture")]
        public byte[] ProfilePicture { get; set; }
        // add foreign keys here for Employer 
        public int? EmployerId { get; set; }
        [ForeignKey("EmployerId")]
        public Employer Employer { get; set; }
        //foreign key with candidate 

        //relationship with messages 
        public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }



    }

    public class UserProfile : ApplicationUser
     {

        public int? CandidateId { get; set; }
       
        public string Skill { get; set; }
        //move rating to user 
        public double Rating { get; set; }

        

        public string? CompanyName { get; set; }


    }
    
}
