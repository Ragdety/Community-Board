using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs.CommunicationDTOs;
using CommunityBoard.Core.DTOs.Responses;
using CommunityBoard.Core.Models.CommunicationModels;

namespace CommunityBoard.Core.Interfaces.Clients
{
    public interface IChatClient
    {
        Task<List<Chat>> GetUserChatsAsync();
        Task<Chat> GetUserChatAsync(int chatId);
        Task<ChatResponse> CreateUserChatAsync(int userId);
        Task<bool> SendMessageAsync(int chatId, CreateMessageDto message);
    }
}