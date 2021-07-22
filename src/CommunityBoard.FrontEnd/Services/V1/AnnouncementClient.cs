using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CoreModels;
using CommunityBoard.FrontEnd.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs.Responses;

namespace CommunityBoard.FrontEnd.Services.V1
{
	public class AnnouncementClient : BaseClient, IAnnouncementClient
    {
        public AnnouncementClient(
            HttpClient client, 
            IHttpContextAccessor httpContextAccessor) : base(client, httpContextAccessor)
        {
            //For now, add it to header to be able to see current logged in announcements
            _httpClient.AddTokenToHeader(_httpContextAccessor.HttpContext.Request.Cookies["JWToken"]);
        }
        
        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Announcements.GetAll);

            if (response.StatusCode == HttpStatusCode.NotFound ||
                !response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsAsync<Response<List<Announcement>>>();
            return content.Data;
        }

        public async Task<List<Announcement>> GetAnnouncementsByNameAsync(string name)
		{
            var response = await _httpClient.GetAsync(
                ApiRoutes.Announcements.GetByName
                .Replace("{announcementName}", name));

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var content = await response.Content.ReadAsAsync<Response<List<Announcement>>>();
            return content.Data;
        }

        public async Task<Tuple<List<Announcement>, string>> 
            GetUserAnnouncementsAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Announcements.GetFromUser);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new Tuple<List<Announcement>, string>(
                        null, "You do not have any announcements yet. Start by making one!");
                case HttpStatusCode.Unauthorized:
                    return new Tuple<List<Announcement>, string>(
                        null, "You do not have access to this user's announcements!");
            }

            if(!response.IsSuccessStatusCode)
            {
                return new Tuple<List<Announcement>, string>(
                    null, "Something went wrong when retrieving announcements");
            }

            var content = await response.Content.ReadAsAsync<Response<List<Announcement>>>();
            return new Tuple<List<Announcement>, string>(content.Data,"");
        }

        public async Task<bool> CreateAnnouncementAsync(CreateAnnouncementDto announcement)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Announcements.Create, announcement);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAnnouncementAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                ApiRoutes.Announcements.Delete.Replace("{announcementId}", id.ToString()));

            return response.IsSuccessStatusCode;
        }

        public async Task<Announcement> GetAnnouncementByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(
                    ApiRoutes.Announcements.Get.Replace("{announcementId}", id.ToString()));

            if (response.StatusCode == HttpStatusCode.NotFound ||
                !response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsAsync<Response<Announcement>>();
            return content.Data;
        }

        public async Task<bool> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto announcement)
        {
            var response = await _httpClient.PutAsJsonAsync(
                ApiRoutes.Announcements.Update.Replace("{announcementId}", id.ToString()), announcement);
            return response.IsSuccessStatusCode;
        }
    }
}