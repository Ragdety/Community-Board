using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs.CommunicationDTOs;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CommunicationModels;
using CommunityBoard.FrontEnd.Extensions;
using Microsoft.AspNetCore.Http;

namespace CommunityBoard.FrontEnd.Services.V1
{
    public class ChatClient : IChatClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.AddTokenToHeader(_httpContextAccessor.HttpContext.Request.Cookies["JWToken"]);
        }
        
        public async Task<List<Chat>> GetUserChats()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Chats.GetAllUserChats);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return null;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsAsync<List<Chat>>();
            return content ?? new List<Chat>(); //If no content return empty list (NOT NULL)
        }

        public async Task<Chat> GetUserChat(int chatId)
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Chats.GetChat.Replace("{chatId}", chatId.ToString()));
            if(!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadAsAsync<Chat>();
        }

        public async Task<bool> CreateUserChat(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SendMessage(int chatId, CreateMessageDto message)
        {
            throw new System.NotImplementedException();
        }
    }
}