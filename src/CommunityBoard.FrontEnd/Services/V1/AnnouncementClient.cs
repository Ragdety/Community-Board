using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models;
using CommunityBoard.FrontEnd.Extensions;
using Microsoft.AspNetCore.Http;
using System;
//using CommunityBoard.FrontEnd.Extensions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

            if (response.StatusCode == HttpStatusCode.NotFound ||
                !response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsAsync<List<Announcement>>();
        }

        public async Task<Tuple<List<Announcement>, string>> 
            GetUserAnnouncementsAsync(int userId, string token)
        {
            //For now, add it to header to be able to see current logged in announcements
            _httpClient.AddTokenToHeader(token);
            var response = await _httpClient.GetAsync(ApiRoutes.Announcements.GetFromUser);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new Tuple<List<Announcement>, string>(
                    null, "You do not have any announcements yet. Start by making one!");
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new Tuple<List<Announcement>, string>(
                    null, "You do not have access to this user's announcements!");
            }
            else if(!response.IsSuccessStatusCode)
            {
                return new Tuple<List<Announcement>, string>(
                    null, "Something went wrong when retrieving announcements");
            }

            return new Tuple<List<Announcement>, string>(
                await response.Content.ReadAsAsync<List<Announcement>>(),"");
        }

        public async Task<bool> CreateAnnouncementAsync(CreateAnnouncementDto announcement, string token)
        {
            _httpClient.AddTokenToHeader(token);
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Announcements.Create, 
                new CreateAnnouncementDto
                {
                    Name = announcement.Name,
                    Type = announcement.Type,
                    Description = announcement.Description,
                    Image = announcement.Image
                });

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAnnouncementAsync(int id, string token)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Announcement> GetAnnouncementByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> UpdateAnnouncementAsync(UpdateAnnouncementDto announcement, string token)
        {
            throw new System.NotImplementedException();
        }
    }
}