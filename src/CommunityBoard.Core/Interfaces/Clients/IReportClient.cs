using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces.Clients
{
	public interface IReportClient
	{
        Task<List<Report>> GetReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
        Task<Tuple<List<Report>, string>> GetAnnouncementReportsAsync(int announcementId);
        Task<bool> CreateReportAsync(int announcementId, CreateReportDto report);
        Task<bool> DeleteReportAsync(int id, string token);
    }
}