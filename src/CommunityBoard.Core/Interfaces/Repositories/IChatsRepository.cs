using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.Models.CommunicationModels;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IChatsRepository : IGenericRepository<Chat>
    {
        Task<bool> JoinChat(int chatId, int userId);
        Task<bool> CreateUserChat(Chat chat, int rootUserId, int targetUserId);
        Task<IEnumerable<Chat>> FindAllUserChatsAsync(int userId);
        Task<bool> DeleteUserChat(int chatId);
        bool IsUserInChat(Chat chat, int userId);
        Task<Tuple<bool, Chat>> UsersHaveChat(int rootUserId, int userId);
    }
}