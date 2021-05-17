using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.BackEnd.Data;
using CommunityBoard.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CommunityBoard.BackEnd.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        
        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<bool> CreateAsync(TEntity entity)
        {
            await _db.Set<TEntity>().AddAsync(entity);
            var created = await _db.SaveChangesAsync();
            return created > 0;
        }

        public async Task<IList<TEntity>> FindAllAsync()
        {
            return await _db
                .Set<TEntity>()
                .ToListAsync();
        }

        public async Task<TEntity> FindByIdAsync(object id)
        {
            return await _db
                .Set<TEntity>()
                .FindAsync(id);
        }

        public async Task<bool> UpdateAsync(TEntity entityToUpdate)
        {
            _db.Set<TEntity>().Update(entityToUpdate);
            var updated = await _db.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entityToDelete)
        {
            _db.Set<TEntity>().Remove(entityToDelete);
            var deleted = await _db.SaveChangesAsync();
            return deleted > 0;
        }
    }
}