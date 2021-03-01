using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        public Task<bool> CreateAsync(TEntity entity);
        public Task<IList<TEntity>> FindAllAsync();
        public Task<TEntity> FindAsync(object id);
        public Task<bool> UpdateAsync(TEntity entityToUpdate);
        public Task<bool> DeleteAsync(object id);
    }
}