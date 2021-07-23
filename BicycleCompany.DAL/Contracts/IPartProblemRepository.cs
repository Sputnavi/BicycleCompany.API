using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartProblemRepository
    {
        Task<IEnumerable<PartProblem>> GetPartProblemsAsync(Guid clientId, Guid id);
        Task<PartProblem> GetPartProblemAsync(Guid clientId, Guid id);
        Task CreatePartProblemAsync(Guid clientId, Guid id, PartProblem partProblem);
        Task DeletePartProblemAsync(PartProblem partProblem);
    }
}
