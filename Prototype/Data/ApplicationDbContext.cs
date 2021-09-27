using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prototype.Models;

namespace Prototype.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Skill> Skills { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ChatMessage>(entity =>
            {
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatMessagesFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatMessagesToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            builder.Entity<Skill>().HasData(

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
            //rename tables for identity
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }


    }
}
