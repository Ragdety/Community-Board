using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
            var announcement = await FindById((int)id);
            _db.Remove(announcement);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<IList<Announcement>> FindAllAsync()
        {
            return await _db.Announcements.ToListAsync();
        }

        public async Task<Announcement> FindById(object id)
        {
            return await _db.Announcements.FirstOrDefaultAsync(a => a.Id == (int)id);
        }

        public async Task<bool> UpdateAsync(Announcement announcementToUpdate)
        {
            _db.Announcements.Update(announcementToUpdate);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<IList<Announcement>> FindAllUserAnnouncements(int userId)
        {
            return await _db.Announcements
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UserOwnsAnnouncementAsync(int announcementId, int userId)
        {
            var announcement = 
                await _db.Announcements.AsNoTracking().SingleOrDefaultAsync(a => a.Id == announcementId);

            if (announcement == null)
                return false;

            if (announcement.UserId != userId)
                return false;

            return true;
        }
    }
}