using CommunityBoard.BackEnd.Data;
using CommunityBoard.BackEnd.Repositories;
using CommunityBoard.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommunityBoard.UnitTests
{
    public class DatabaseFixture 
    {
        private ApplicationDbContext _db;
        public AnnouncementsRepository _repo;

        public DatabaseFixture()
        {
            //Test Db
            //var dbContextOptions =
            //    new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
            //        "Data Source=DESKTOP-8HDJMKA\\RAGDETYSERVER;Initial Catalog=CommunityBoardTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            //In memory
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            _db = new ApplicationDbContext(dbContextOptions.Options);
            _db.Database.EnsureCreated();
            Seed();
            _repo = new AnnouncementsRepository(_db);
        }

        

        //Method to add dummy data to temp database if we want to
        public void Seed()
        {
            //Will delete test db 
            //_db.Database.EnsureDeleted();

            for (int i = 0; i < 5; i++)
            {
                _db.Users.AddRange(
                new User
                {
                    Id = i,
                    FirstName = $"Test First {i}",
                    LastName = $"Test Last {i}",
                    Email = $"test{i}@test.com",
                    UserName = $"Test{i}",
                    DateRegistered = DateTime.UtcNow
                });
            }

            _db.Announcements.AddRange(
                new Announcement
                {
                    Name = "Unit Test 1",
                    Type = Core.Enums.AnnouncementType.Sale,
                    Description = "Unit Test Description 1",
                    Image = null,
                    CreatedAt = DateTime.UtcNow,
                    UserId = 1
                },
                new Announcement
                {
                    Name = "Unit Test 2",
                    Type = Core.Enums.AnnouncementType.Other,
                    Description = "Unit Test Description 2",
                    Image = null,
                    CreatedAt = DateTime.UtcNow,
                    UserId = 1
                },
                new Announcement
                {
                    Name = "Unit Test 3",
                    Type = Core.Enums.AnnouncementType.JobOffer,
                    Description = "Unit Test Description 3",
                    Image = null,
                    CreatedAt = DateTime.UtcNow,
                    UserId = 2
                },
                new Announcement
                {
                    Name = "Unit Test 4",
                    Type = Core.Enums.AnnouncementType.Club,
                    Description = "Unit Test Description 4",
                    Image = null,
                    CreatedAt = DateTime.UtcNow,
                    UserId = 2
                });

            _db.SaveChanges();
        }
    }
}
