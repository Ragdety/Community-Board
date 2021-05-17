using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models.CommunicationModels;

namespace CommunityBoard.BackEnd.Repositories.CommunicationRepos
{
    public class ChatsRepository : IGenericRepository<Chat>
    {
        public Task<bool> CreateAsync(Chat entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Chat>> FindAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Chat> FindByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(Chat entityToUpdate)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new System.NotImplementedException();
        }
    }
}