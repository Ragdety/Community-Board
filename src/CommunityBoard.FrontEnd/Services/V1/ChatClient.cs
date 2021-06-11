using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
        
        public Task<List<Chat>> GetUserChats()
        {
            throw new System.NotImplementedException();
        }

        public Task<Chat> GetUserChat(int chatId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CreateUserChat(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SendMessage(int chatId, CreateMessageDto message)
        {
            throw new System.NotImplementedException();
        }
    }
}