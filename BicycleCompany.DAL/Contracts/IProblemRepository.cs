using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IProblemRepository
    {
        Task<PagedList<Problem>> GetProblemListAsync(ProblemParameters problemParameters);
        Task<Problem> GetProblemAsync(Guid id);
        Task CreateProblemAsync(Problem problem);
        Task DeleteProblemAsync(Problem problem);
        Task UpdateProblemAsync(Problem problem);

        Task<PagedList<Problem>> GetProblemListForClientAsync(Guid clientId, ProblemParameters problemParameters);
        Task<Problem> GetProblemForClientAsync(Guid clientId, Guid problemId);
    }
}
