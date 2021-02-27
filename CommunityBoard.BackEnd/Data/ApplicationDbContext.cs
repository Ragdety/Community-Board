using System;
using System.Collections.Generic;
using System.Text;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
