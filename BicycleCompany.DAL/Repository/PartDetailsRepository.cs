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

        public Task CreatePartDetailAsync(Guid clientId, Guid problemId, PartDetails partProblem)
        {
            partProblem.ProblemId = problemId;
            return CreateAsync(partProblem);
        }

        public Task DeletePartDetailAsync(PartDetails partProblem) => DeleteAsync(partProblem);

        public async Task<PartDetails> GetPartDetailAsync(Guid clientId, Guid problemId, Guid id) =>
            await FindByCondition(pd => pd.Problem.ClientId.Equals(clientId) && pd.ProblemId.Equals(problemId) && pd.Id == id)
                .Include(pd => pd.Part)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<PartDetails>> GetPartDetailListAsync(Guid clientId, Guid problemId)
        {
            var parts = await FindByCondition(pd => pd.Problem.ClientId.Equals(clientId) && pd.ProblemId.Equals(problemId))
                .Include(pd => pd.Part)
                .OrderBy(pd => pd.Amount)
                .ToListAsync();

            return parts;
        }
    }
}
