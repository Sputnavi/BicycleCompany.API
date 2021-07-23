using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class ProblemRepository : RepositoryBase<Problem>, IProblemRepository
    {
        public ProblemRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreateProblemAsync(Guid clientId, Problem problem)
        {
            problem.ClientId = clientId;
            return CreateAsync(problem);
        }

        public Task DeleteProblemAsync(Problem problem) => DeleteAsync(problem);

        public Task UpdateProblemAsync(Problem problem) => UpdateAsync(problem);

        public async Task<Problem> GetProblemAsync(Guid clientId, Guid id) => 
            await FindByCondition(p => p.Id.Equals(id) && p.ClientId.Equals(clientId))
            .Include(p => p.PartProblems)
                .ThenInclude(pp => pp.Part)
            .Include(p => p.Bicycle)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Problem>> GetProblemsAsync(Guid clientId) =>
            await FindByCondition(p => p.ClientId.Equals(clientId))
            .Include(p => p.Bicycle)
            .Include(p => p.PartProblems)
                .ThenInclude(pp => pp.Part)
            .OrderBy(p => p.Stage)
            .ToListAsync();
    }
}
