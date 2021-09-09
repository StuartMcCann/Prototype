using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public enum Level
    {
        [Display(Name = "Entry", Description = "1-2 years Experience")]
        Entry,
        [Display(Name = "Intermediate", Description = "3-7 years Experience")]
        Intermediate,

        [Display(Name = "Expert", Description = "7+ years Experience")]
        Expert
    }
}
