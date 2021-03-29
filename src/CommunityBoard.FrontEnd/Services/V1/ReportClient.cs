using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Services.V1
{
	public class ReportClient : IReportClient
	{
		private readonly HttpClient _httpClient;

		public ReportClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<bool> CreateReportAsync(int announcementId, CreateReportDto report)
		{
			var response = await _httpClient.PostAsJsonAsync(
				ApiRoutes.Reports.Create.Replace("{announcementId}", announcementId.ToString()),
				report);

			return response.IsSuccessStatusCode;
		}

		public Task<bool> DeleteReportAsync(int id, string token)
		{
			throw new NotImplementedException();
		}

		public Task<Tuple<List<Report>, string>> GetAnnouncementReportsAsync(int announcementId)
		{
			throw new NotImplementedException();
		}

		public Task<Report> GetReportByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<List<Report>> GetReportsAsync()
		{
			throw new NotImplementedException();
		}
	}
}