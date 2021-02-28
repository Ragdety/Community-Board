using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // For int Primary Key
            builder.Entity<User>()
                .Property(p => p.Id)
                .UseIdentityColumn();

            builder.Entity<Role>()
                .Property(p => p.Id)
                .UseIdentityColumn();

            builder.Entity<Announcement>()
                .Property(p => p.Type)
                .HasColumnType("nvarchar(50)");
        }
    }
}
