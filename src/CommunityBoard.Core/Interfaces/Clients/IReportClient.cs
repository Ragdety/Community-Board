using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Models.CoreModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Clients
{
	public interface IReportClient
	{
        Task<List<Report>> GetReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task<List<Report>> GetAnnouncementReportsAsync(int announcementId);
        Task<bool> CreateReportAsync(int announcementId, CreateReportDto report);
        Task<bool> DeleteReportAsync(int id);
    }
}