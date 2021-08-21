using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prototype.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Data
{
    public class ContextSeed
    {
        //This method seeds Required user roles to the Database 
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Employer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Candidate.ToString()));

        }

        //this method seeds a default super user who will have admin priveledges for the website 
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "smccann74@qub.ac.uk",
                FirstName = "Super",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Candidate.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Employer.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }


        public static async Task SeedSkills(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(

               new Skill { SkillId = 1, SkillName = "Java" },
               new Skill { SkillId = 2, SkillName = "C#" },
               new Skill { SkillId = 3, SkillName = "Javascript" },
                new Skill { SkillId = 4, SkillName = "SQL" },
                new Skill { SkillId = 5, SkillName = "Python" },
                new Skill { SkillId = 6, SkillName = "C++" },
                new Skill { SkillId = 7, SkillName = "Go" },
                new Skill { SkillId = 8, SkillName = "R" },
                new Skill { SkillId = 9, SkillName = "PHP" },
                new Skill { SkillId = 10, SkillName = "Perl" },
                new Skill { SkillId = 11, SkillName = "Ruby" },
                new Skill { SkillId = 12, SkillName = "Html" },
                new Skill { SkillId = 13, SkillName = "Bootstrap" },
                new Skill { SkillId = 14, SkillName = "Git" },
                new Skill { SkillId = 15, SkillName = "Elastic Search" },
                new Skill { SkillId = 16, SkillName = "Typscript" },
                new Skill { SkillId = 17, SkillName = "Node.Js" },
                new Skill { SkillId = 18, SkillName = "Angular" },
                new Skill { SkillId = 19, SkillName = ".Net" },
                new Skill { SkillId = 20, SkillName = "Kolen" },
                new Skill { SkillId = 21, SkillName = "React.Js" },
                new Skill { SkillId = 22, SkillName = "MatLab" }





               );
        }
    }
}
