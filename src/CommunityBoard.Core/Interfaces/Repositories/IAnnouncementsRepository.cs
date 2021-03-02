using CommunityBoard.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IAnnouncementsRepository : IGenericRepository<Announcement>
    {
        public Task<IList<Announcement>> FindAllUserAnnouncements(int userId);
        Task<bool> UserOwnsAnnouncementAsync(int announcementId, int userId);
    }
}
