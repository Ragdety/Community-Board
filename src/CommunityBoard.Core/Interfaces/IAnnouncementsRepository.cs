using CommunityBoard.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces
{
    public interface IAnnouncementsRepository : IGenericRepository<Announcement>
    {
        public Task<IList<Announcement>> FindAllUserAnnouncements(User user);
    }
}
