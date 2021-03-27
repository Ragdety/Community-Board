using CommunityBoard.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IReportsRepository : IGenericRepository<Report>
    {
        Task<IList<Report>> FindAllReportsFromAnnouncement(int announcementId);
    }
}