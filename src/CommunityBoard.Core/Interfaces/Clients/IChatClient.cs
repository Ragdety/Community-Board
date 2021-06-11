using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.DTOs.CommunicationDTOs;
using CommunityBoard.Core.Models.CommunicationModels;

namespace CommunityBoard.Core.Interfaces.Clients
{
    public interface IChatClient
    {
        Task<List<Chat>> GetUserChats();
        Task<Chat> GetUserChat(int chatId);
        Task<bool> CreateUserChat(int userId);
        Task<bool> SendMessage(int chatId, CreateMessageDto message);
    }
}