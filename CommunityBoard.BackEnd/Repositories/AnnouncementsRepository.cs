using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        public Task<Announcement> CreateAsync(Announcement entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<Announcement> DeleteAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Announcement> DeleteUserAnnouncement(int announcementId, User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Announcement>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Announcement> FindAllUserAnnouncements(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Announcement> FindAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(object id)
        {
            throw new System.NotImplementedException();
        }
    }
}
