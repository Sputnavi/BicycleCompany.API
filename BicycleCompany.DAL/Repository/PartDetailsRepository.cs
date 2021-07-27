using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class PartDetailsRepository : RepositoryBase<PartDetails>, IPartDetailsRepository
    {
        public PartDetailsRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreatePartDetailAsync(PartDetails partProblem) => CreateAsync(partProblem);

        public Task DeletePartDetailAsync(PartDetails partProblem) => DeleteAsync(partProblem);

        public async Task<PartDetails> GetPartDetailAsync(Guid problemId, Guid partId) =>
            await FindByCondition(pd => pd.ProblemId.Equals(problemId) && pd.PartId.Equals(partId))
                .Include(pd => pd.Part)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<PartDetails>> GetPartDetailListAsync(Guid problemId)
        {
            var parts = await FindByCondition(pd => pd.ProblemId.Equals(problemId))
                .Include(pd => pd.Part)
                .OrderBy(pd => pd.Amount)
                .ToListAsync();

            return parts;
        }
    }
}
