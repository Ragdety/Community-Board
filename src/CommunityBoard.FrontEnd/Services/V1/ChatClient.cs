using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Contracts.V1;
using CommunityBoard.Core.DTOs.CommunicationDTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Interfaces.Clients;
using CommunityBoard.Core.Models.CommunicationModels;
using CommunityBoard.FrontEnd.Extensions;
using Microsoft.AspNetCore.Http;

namespace CommunityBoard.FrontEnd.Services.V1
{
    public class ChatClient : BaseClient, IChatClient
    {
        public ChatClient(
            HttpClient httpClient, 
            IHttpContextAccessor httpContextAccessor) : base(httpClient, httpContextAccessor)
        {
            _httpClient.AddTokenToHeader(_httpContextAccessor.HttpContext.Request.Cookies["JWToken"]);
        }
        
        public async Task<List<Chat>> GetUserChatsAsync()
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Chats.GetAllUserChats);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return null;

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsAsync<List<Chat>>();
            return content ?? new List<Chat>(); //If no content return empty list (NOT NULL)
        }

        public async Task<Chat> GetUserChatAsync(int chatId)
        {
            var response = await _httpClient.GetAsync(ApiRoutes.Chats.GetChat.Replace("{chatId}", chatId.ToString()));
            if(!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadAsAsync<Chat>();
        }

        public async Task<ChatResponse> CreateUserChatAsync(int userId)
        {
            string url = ApiRoutes.Chats.CreateUserChat.Replace("{userId}", userId.ToString());
            var response = await _httpClient.PostAsync(url, null);
            return await response.Content.ReadAsAsync<ChatResponse>();
        }

        public async Task<bool> SendMessageAsync(int chatId, CreateMessageDto message)
        {
            string url = ApiRoutes.Messages.Send.Replace("{chatId}", chatId.ToString());
            var response = await _httpClient.PostAsJsonAsync(url, message);
            return response.IsSuccessStatusCode;
        }
    }
}