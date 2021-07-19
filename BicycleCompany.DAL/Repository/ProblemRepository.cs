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

        public Task CreateProblemAsync(Problem problem) => CreateAsync(problem);

        public Task DeleteProblemAsync(Problem problem) => DeleteAsync(problem);

        public Task UpdateProblemAsync(Problem problem) => UpdateAsync(problem);

        public async Task<Problem> GetProblemAsync(Guid id) => 
            await FindByCondition(p => p.Id.Equals(id)).SingleOrDefaultAsync();

        public async Task<IEnumerable<Problem>> GetProblemsAsync() =>
            await FindAll()
            .OrderBy(p => p.Stage)
            .ToListAsync();
    }
}
