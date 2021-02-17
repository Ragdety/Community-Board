using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task<IList<TEntity>> FindAllAsync();
        public Task<TEntity> FindAsync(object id);
        public Task<bool> UpdateAsync(object id);
        public Task<TEntity> DeleteAsync(object id);
    }
}