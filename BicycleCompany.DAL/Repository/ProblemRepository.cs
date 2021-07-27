using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions;
using BicycleCompany.Models.Request.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class ProblemRepository : RepositoryBase<Problem>, IProblemRepository
    {
        public ProblemRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreateProblemAsync(Problem problem)
        {
            return CreateAsync(problem);
        }

        public Task DeleteProblemAsync(Problem problem) => DeleteAsync(problem);

        public Task UpdateProblemAsync(Problem problem) => UpdateAsync(problem);

        public async Task<Problem> GetProblemForClientAsync(Guid clientId, Guid problemId) => 
            await FindByCondition(p => p.Id.Equals(problemId) && p.ClientId.Equals(clientId))
            .Include(p => p.PartDetails)
                .ThenInclude(pp => pp.Part)
            .Include(p => p.Bicycle)
            .SingleOrDefaultAsync();

        public async Task<PagedList<Problem>> GetProblemListForClientAsync(Guid clientId, ProblemParameters problemParameters)
        {
            var problems = await FindByCondition(p => p.ClientId.Equals(clientId))
                .Search(problemParameters.SearchTerm)
                .Include(p => p.Bicycle)
                .Include(p => p.PartDetails)
                    .ThenInclude(pp => pp.Part)
                .Sort(problemParameters.OrderBy)
                .ToListAsync();

            return PagedList<Problem>.ToPagedList(problems, problemParameters.PageNumber, problemParameters.PageSize);
        }

        public async Task<PagedList<Problem>> GetProblemListAsync(ProblemParameters problemParameters)
        {
            var problems = await FindAll()
                .Search(problemParameters.SearchTerm)
                .Include(p => p.Bicycle)
                .Include(p => p.PartDetails)
                    .ThenInclude(pp => pp.Part)
                .Sort(problemParameters.OrderBy)
                .ToListAsync();

            return PagedList<Problem>.ToPagedList(problems, problemParameters.PageNumber, problemParameters.PageSize);
        }

        public async Task<Problem> GetProblemAsync(Guid id) =>
            await FindByCondition(p => p.Id.Equals(id))
            .Include(p => p.PartDetails)
                .ThenInclude(pp => pp.Part)
            .Include(p => p.Bicycle)
            .SingleOrDefaultAsync();
    }
}
