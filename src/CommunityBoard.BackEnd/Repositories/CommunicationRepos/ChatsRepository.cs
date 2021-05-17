using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Repositories.CommunicationRepos
{
    public class ChatsRepository : GenericRepository<Chat>, IChatsRepository
    {
        private readonly DbSet<Chat> _chats;
        
        public ChatsRepository(ApplicationDbContext db) : base(db)
        {
            _chats = db.Set<Chat>();
        }
    }
}