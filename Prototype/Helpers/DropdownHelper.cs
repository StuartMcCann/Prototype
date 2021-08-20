using Prototype.Data;
using Prototype.Models;
using System.Collections.Generic;

namespace Prototype.Helpers
{
    public static class DropdownHelper
    {
        

        public static string DisplaySkills(List<Skill> skills)
        {
            var output = "<h5>";

            foreach (var skill in skills)
            {
                output += "< span class='badge skill'>" + skill.SkillName + "</span>";
            }


            return output+"</h5>";
        }




    }
}
