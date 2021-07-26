using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class PartProblemRepository : RepositoryBase<PartProblem>, IPartProblemRepository
    {
        public PartProblemRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreatePartProblemAsync(Guid clientId, Guid problemId, PartProblem partProblem)
        {
            partProblem.ProblemId = problemId;
            return CreateAsync(partProblem);
        }

        public Task DeletePartProblemAsync(PartProblem partProblem) => DeleteAsync(partProblem);

        public async Task<PartProblem> GetPartProblemAsync(Guid clientId, Guid problemId, Guid id) =>
            await FindByCondition(pp => pp.Problem.ClientId.Equals(clientId) && pp.ProblemId.Equals(problemId) && pp.Id == id)
                .Include(pp => pp.Part)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<PartProblem>> GetPartProblemsAsync(Guid clientId, Guid problemId)
        {
            var parts = await FindByCondition(pp => pp.Problem.ClientId.Equals(clientId) && pp.ProblemId.Equals(problemId))
                .Include(pp => pp.Part)
                .OrderBy(pp => pp.Amount)
                .ToListAsync();

            return parts;
        }
    }
}
