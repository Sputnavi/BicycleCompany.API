using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions;
using BicycleCompany.Models.Request.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Part> GetPartAsync(Guid id) => 
            await FindByCondition(p => p.Id.Equals(id)).SingleOrDefaultAsync();

        public async Task<PagedList<Part>> GetPartsAsync(PartParameters partParameters)
        {
            var parts = await FindAll()
                .FilterParts(partParameters.MinAmount, partParameters.MaxAmount)
                .Search(partParameters.SearchTerm)
                .Sort(partParameters.OrderBy)
                .ToListAsync();

            return PagedList<Part>.ToPagedList(parts, partParameters.PageNumber, partParameters.PageSize);
        }
    }
}
