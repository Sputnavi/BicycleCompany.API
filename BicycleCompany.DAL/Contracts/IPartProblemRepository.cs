using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartProblemRepository
    {
        Task<IEnumerable<PartProblem>> GetPartProblemsAsync(Guid clientId, Guid problemId);
        Task<PartProblem> GetPartProblemAsync(Guid clientId, Guid problemId, Guid id);
        Task CreatePartProblemAsync(Guid clientId, Guid problemId, PartProblem partProblem);
        Task DeletePartProblemAsync(PartProblem partProblem);
    }
}
