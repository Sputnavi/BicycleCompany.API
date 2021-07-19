using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class BicycleRepository : RepositoryBase<Bicycle>, IBicycleRepository
    {
        public BicycleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreateBicycleAsync(Bicycle bicycle) => CreateAsync(bicycle);

        public Task DeleteBicycleAsync(Bicycle bicycle) => DeleteAsync(bicycle);

        public Task UpdateBicycleAsync(Bicycle bicycle) => UpdateAsync(bicycle);

        public async Task<Bicycle> GetBicycleAsync(Guid id) => 
            await FindByCondition(b => b.Id.Equals(id)).SingleOrDefaultAsync();

        public async Task<IEnumerable<Bicycle>> GetBicyclesAsync() => 
            await FindAll()
            .OrderBy(b => b.Name)
            .ToListAsync();
    }
}
