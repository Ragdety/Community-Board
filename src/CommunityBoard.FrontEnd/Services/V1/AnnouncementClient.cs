using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
//using CommunityBoard.FrontEnd.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommunityBoard.FrontEnd.Services.V1
{
    public class AnnouncementClient : IAnnouncementClient
    {
        private readonly HttpClient _httpClient;

        public AnnouncementClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Announcements.GetAll);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<List<Announcement>>();
        }

        public async Task<Announcement> GetUserAnnouncementAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CreateAnnouncementAsync(CreateAnnouncementDto announcement)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Announcement> GetAnnouncementByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateAnnouncementAsync(UpdateAnnouncementDto announcement)
        {
            throw new System.NotImplementedException();
        }
    }
}
