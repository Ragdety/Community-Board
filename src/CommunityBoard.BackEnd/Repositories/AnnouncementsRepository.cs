using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private readonly ApplicationDbContext _db;
        public AnnouncementsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(Announcement announcement)
        {
            await _db.Announcements.AddAsync(announcement);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var announcement = await FindAsync(id);
            _db.Remove(announcement);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<IList<Announcement>> FindAllAsync()
        {
            return await _db.Announcements.ToListAsync();
        }

        public async Task<Announcement> FindAsync(object id)
        {
            return await _db.Announcements.SingleOrDefaultAsync(a => a.Id == (int)id);
        }

        public async Task<bool> UpdateAsync(Announcement announcementToUpdate)
        {
            _db.Announcements.Update(announcementToUpdate);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public Task<IList<Announcement>> FindAllUserAnnouncements(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
