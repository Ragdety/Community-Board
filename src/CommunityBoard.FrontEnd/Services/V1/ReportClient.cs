using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CoreModels;
using CommunityBoard.FrontEnd.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CommunityBoard.FrontEnd.Services.V1
{
	public class ReportClient : IReportClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ReportClient(
			HttpClient httpClient,
			IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = httpClient;
			_httpContextAccessor = httpContextAccessor;
			_httpClient.AddTokenToHeader(_httpContextAccessor.HttpContext.Request.Cookies["JWToken"]);
		}

		public async Task<bool> CreateReportAsync(int announcementId, CreateReportDto report)
		{
			var response = await _httpClient.PostAsJsonAsync(
				ApiRoutes.Reports.Create.Replace("{announcementId}", announcementId.ToString()),
				report);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteReportAsync(int id)
		{
			var response = await _httpClient.DeleteAsync(
				ApiRoutes.Reports.Delete.Replace("{reportId}", id.ToString()));

			return response.IsSuccessStatusCode;
		}

		public async Task<List<Report>> GetAnnouncementReportsAsync(int announcementId)
		{
			var response = await _httpClient.GetAsync(
				ApiRoutes.Reports.GetAllFromAnnouncement.Replace(
					"{announcementId}", announcementId.ToString()));

			if(!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadAsAsync<List<Report>>();
		}

		public async Task<Report> GetReportByIdAsync(int id)
		{
			var response = await _httpClient.GetAsync(
				ApiRoutes.Reports.Get.Replace(
					"{reportId}", id.ToString()));

			if (!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadAsAsync<Report>();
		}

		public async Task<List<Report>> GetReportsAsync()
		{
			var response = await _httpClient.GetAsync(ApiRoutes.Reports.GetAll);
			if(!response.IsSuccessStatusCode)
				return null;

			return await response.Content.ReadAsAsync<List<Report>>();
		}
	}
}