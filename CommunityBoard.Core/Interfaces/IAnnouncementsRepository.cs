﻿using CommunityBoard.Core.Models;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces
{
    public interface IAnnouncementsRepository : IGenericRepository<Announcement>
    {
        public Task<Announcement> FindAllUserAnnouncements(User user);
        public Task<Announcement> DeleteUserAnnouncement(int announcementId, User user);
    }
}