using Microsoft.AspNetCore.Identity;
using Prototype.Data;
using Prototype.Enums;
using Prototype.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.Helpers
{
    public class DropdownHelper
    {

       
        private readonly ApplicationDbContext _db;
        public DropdownHelper( ApplicationDbContext applicationDbContext)
        {
           
            _db = applicationDbContext;
        }

        


    }
}
