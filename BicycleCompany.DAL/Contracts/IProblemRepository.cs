using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IProblemRepository
    {
        Task<PagedList<Problem>> GetProblemListAsync(Guid clientId, ProblemParameters problemParameters);
        Task<Problem> GetProblemAsync(Guid clientId, Guid id);
        Task CreateProblemAsync(Guid clientId, Problem problem);
        Task DeleteProblemAsync(Problem problem);
        Task UpdateProblemAsync(Problem problem);
    }
}
