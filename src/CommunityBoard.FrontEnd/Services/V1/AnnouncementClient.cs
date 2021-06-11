﻿using CommunityBoard.BackEnd.Contracts.V1;
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

namespace CommunityBoard.FrontEnd.Services.V1
{
	public class AnnouncementClient : IAnnouncementClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnnouncementClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            
            //For now, add it to header to be able to see current logged in announcements
            _httpClient.AddTokenToHeader(_httpContextAccessor.HttpContext.Request.Cookies["JWToken"]);
        }
        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Announcements.GetAll);

            if (response.StatusCode == HttpStatusCode.NotFound ||
                !response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsAsync<List<Announcement>>();
        }

        public async Task<List<Announcement>> GetAnnouncementsByNameAsync(string name)
		{
            var response = await _httpClient.GetAsync(
                ApiRoutes.Announcements.GetByName
                .Replace("{announcementName}", name));

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            return await response.Content.ReadAsAsync<List<Announcement>>();
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

            return new Tuple<List<Announcement>, string>(
                await response.Content.ReadAsAsync<List<Announcement>>(),"");
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

            return await response.Content.ReadAsAsync<Announcement>();
        }

        public async Task<bool> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto announcement)
        {
            var response = await _httpClient.PutAsJsonAsync(
                ApiRoutes.Announcements.Update.Replace("{announcementId}", id.ToString()), announcement);
            return response.IsSuccessStatusCode;
        }
    }
}