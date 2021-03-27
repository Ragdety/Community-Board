using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly ApplicationDbContext _db;

		public ReportsRepository(ApplicationDbContext db)
		{
            _db = db;
		}

        public async Task<bool> CreateAsync(Report report)
        {
            await _db.AddAsync(report);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteAsync(object id)
        {
            var report = await FindByIdAsync((int)id);
            _db.Remove(report);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<IList<Report>> FindAllAsync()
        {
            return await _db.Reports
                .OrderByDescending(r => r.ReportDate)
                .ToListAsync();
        }

		public async Task<Report> FindByIdAsync(object id)
        {
            return await _db.Reports.FirstOrDefaultAsync(r => r.Id == (int)id);
        }

        public async Task<IList<Report>> FindAllReportsFromAnnouncement(int announcementId)
        {
            return await _db.Reports
                .Where(r => r.AnnouncementId == announcementId)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Report reportToUpdate)
        {
            _db.Reports.Update(reportToUpdate);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }
    }
}