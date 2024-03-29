﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityBoard.Core.Models.Filters;

namespace CommunityBoard.Core.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<bool> CreateAsync(TEntity entity);
        public Task<IList<TEntity>> FindAllAsync(PaginationFilter paginationFilter = null);
        public Task<TEntity> FindByIdAsync(object id);
        public Task<bool> UpdateAsync(TEntity entityToUpdate);
        public Task<bool> DeleteAsync(TEntity entityToDelete);
    }
}