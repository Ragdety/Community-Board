using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Repositories.CommunicationRepos
{
    public class MessagesRepository : GenericRepository<Message>, IMessagesRepository
    {
        private readonly DbSet<Message> _messages;
        
        public MessagesRepository(ApplicationDbContext db) : base(db)
        {
            _messages = db.Set<Message>();
        }
    }
}