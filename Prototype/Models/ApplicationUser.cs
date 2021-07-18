﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Profile Picture")]
        public byte[] ProfilePicture { get; set; }
        // add foreign keys here for Employer 
        public int? EmployerId { get; set; }
        [ForeignKey("EmployerId")]
        public Employer Employer { get; set; }
        //foreign key with candidate 

        


    }
}
