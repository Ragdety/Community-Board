using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .ToTable("Users");

            builder.Entity<Announcement>()
                .Property(p => p.Type)
                .HasColumnType("nvarchar(50)");
        }
    }
}
