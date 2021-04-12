using CommunityBoard.Core.Enums;
using CommunityBoard.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IAnnouncementsRepository : IGenericRepository<Announcement>
    {
        Task<IList<Announcement>> FindAllUserAnnouncements(int userId);
        Task<bool> UserOwnsAnnouncementAsync(int announcementId, int userId);
        Task<IList<Announcement>> FindAnnouncementsByName(string name);
        Task<IList<Announcement>> FindAnnouncementsByType(AnnouncementType type);
    }
}