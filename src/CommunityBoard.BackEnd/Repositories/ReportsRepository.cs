using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        public Task<Report> CreateAsync(Report entity)
        {
            throw new NotImplementedException();
        }

        public Task<Report> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Report>> FindAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Report> FindAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}
