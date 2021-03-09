using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        public Task<bool> CreateAsync(Report entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Report>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Report> FindByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Report entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
