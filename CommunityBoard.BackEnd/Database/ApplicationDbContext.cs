using CommunityBoard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public ApplicationDbContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
