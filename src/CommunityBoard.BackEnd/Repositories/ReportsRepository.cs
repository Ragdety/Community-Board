using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CoreModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
	public class ReportsRepository : GenericRepository<Report>, IReportsRepository
    {
        private readonly DbSet<Report> _reports;
		public ReportsRepository(ApplicationDbContext db) : base(db)
        {
            _reports = db.Set<Report>();
        }

        public async Task<IList<Report>> FindAllReportsFromAnnouncement(int announcementId)
        {
            return await _reports
                .Where(r => r.AnnouncementId == announcementId)
                .ToListAsync();
        }
    }
}