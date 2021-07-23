using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IProblemRepository
    {
        Task<IEnumerable<Problem>> GetProblemsAsync(Guid clientId);
        Task<Problem> GetProblemAsync(Guid clientId, Guid id);
        Task CreateProblemAsync(Guid clientId, Problem problem);
        Task DeleteProblemAsync(Problem problem);
        Task UpdateProblemAsync(Problem problem);
    }
}
