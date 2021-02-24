using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        public Task<Announcement> CreateAsync(Announcement entity)
        {
            throw new NotImplementedException();
        }

        public Task<Announcement> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<Announcement> DeleteUserAnnouncement(int announcementId, User user)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Announcement>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Announcement> FindAllUserAnnouncements(User user)
        {
            throw new NotImplementedException();
        }

        public Task<Announcement> FindAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}
