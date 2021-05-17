using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
	public class AnnouncementsRepository : GenericRepository<Announcement>, IAnnouncementsRepository
    {
        private readonly DbSet<Announcement> _announcements;
        
        public AnnouncementsRepository(ApplicationDbContext db) : base(db)
        {
            _announcements = db.Set<Announcement>();
        }

        public async Task<IList<Announcement>> FindAllUserAnnouncements(int userId)
        {
            return await _announcements
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<IList<Announcement>> FindAnnouncementsByName(string name)
		{
            if(string.IsNullOrEmpty(name))
			{
                return await FindAllAsync();
            }

            return await _announcements
                .Where(a => a.Name.Contains(name))
                .ToListAsync();
		}

        public async Task<IList<Announcement>> FindAnnouncementsByType(AnnouncementType type)
        {
            return await _announcements
                .Where(a => a.Type == type)
                .ToListAsync();
        }

        public async Task<bool> UserOwnsAnnouncementAsync(int announcementId, int userId)
        {
            var announcement = 
                await _announcements.AsNoTracking().SingleOrDefaultAsync(a => a.Id == announcementId);

            if (announcement == null)
                return false;

            if (announcement.UserId != userId)
                return false;

            return true;
        }
    }
}