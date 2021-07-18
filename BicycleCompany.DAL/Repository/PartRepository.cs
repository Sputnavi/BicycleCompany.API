using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class PartRepository : RepositoryBase<Part>, IPartRepository
    {
        public PartRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreatePartAsync(Part part) => CreateAsync(part);

        public Task DeletePartAsync(Part part) => DeleteAsync(part);

        public Task UpdatePartAsync(Part part) => UpdateAsync(part);

        public async Task<Part> GetPartAsync(Guid id, bool trackChanges) => 
            await FindByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Part>> GetPartsAsync(bool trackChanges) => 
            await FindAll(trackChanges)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}
