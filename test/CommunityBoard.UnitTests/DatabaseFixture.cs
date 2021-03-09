using CommunityBoard.BackEnd.Data;
using CommunityBoard.BackEnd.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace CommunityBoard.UnitTests
{
    public class DatabaseFixture : IDisposable
    {
        private ApplicationDbContext _db;
        public AnnouncementsRepository _repo;

        public DatabaseFixture()
        {
            //Will create a test db
            var dbContextOptions =
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
                    "Data Source=DESKTOP-8HDJMKA\\RAGDETYSERVER;Initial Catalog=CommunityBoardTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            _db = new ApplicationDbContext(dbContextOptions.Options);
            _db.Database.EnsureCreated();

            _repo = new AnnouncementsRepository(_db);
        }

        public void Dispose()
        {
            //Will delete test db after tests
            _db.Database.EnsureDeleted();
        }
    }
}
