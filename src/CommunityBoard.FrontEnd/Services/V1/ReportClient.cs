using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using CommunityBoard.FrontEnd.Extensions;
using System.Collections.Generic;
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

		public async Task<bool> DeleteReportAsync(int id, string token)
		{
			_httpClient.AddTokenToHeader(token);

			var response = await _httpClient.DeleteAsync(
				ApiRoutes.Reports.Delete.Replace("{reportId}", id.ToString()));

			return response.IsSuccessStatusCode;
		}

		public async Task<List<Report>> GetAnnouncementReportsAsync(int announcementId, string token)
		{
			_httpClient.AddTokenToHeader(token);

			var response = await _httpClient.GetAsync(
				ApiRoutes.Reports.GetAllFromAnnouncement.Replace(
					"{announcementId}", announcementId.ToString()));

			if(!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadAsAsync<List<Report>>();
		}

		public async Task<Report> GetReportByIdAsync(int id, string token)
		{
			_httpClient.AddTokenToHeader(token);

			var response = await _httpClient.GetAsync(
				ApiRoutes.Reports.Get.Replace(
					"{reportId}", id.ToString()));

			if (!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadAsAsync<Report>();
		}

		public async Task<List<Report>> GetReportsAsync(string token)
		{
			_httpClient.AddTokenToHeader(token);

			var response = await _httpClient.GetAsync(ApiRoutes.Reports.GetAll);
			if(!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadAsAsync<List<Report>>();
		}
	}
}