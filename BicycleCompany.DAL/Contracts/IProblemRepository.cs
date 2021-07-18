using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IProblemRepository
    {
        Task<IEnumerable<Problem>> GetProblemsAsync(bool trackChanges);
        Task<Problem> GetProblemAsync(Guid id, bool trackChanges);
        Task CreateProblemAsync(Problem problem);
        Task DeleteProblemAsync(Problem problem);
    }
}
