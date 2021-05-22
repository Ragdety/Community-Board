using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.Models.CommunicationModels;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IChatsRepository : IGenericRepository<Chat>
    {
        Task<bool> JoinChat(int chatId, int userId);
        Task<bool> CreateUserChat(int rootUserId, int targetUserId);
        Task<Chat> GetUserChat(int rootUserId, int targetUserId);
        Task<IEnumerable<Chat>> GetAllUserChats(int userId);
        Task<bool> DeleteUserChat(int chatId, int userId);
    }
}