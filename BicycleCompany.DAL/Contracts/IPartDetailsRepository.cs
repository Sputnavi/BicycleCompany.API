using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartDetailsRepository
    {
        Task<IEnumerable<PartDetails>> GetPartDetailListAsync(Guid clientId, Guid problemId);
        Task<PartDetails> GetPartDetailAsync(Guid clientId, Guid problemId, Guid id);
        Task CreatePartDetailAsync(Guid clientId, Guid problemId, PartDetails partProblem);
        Task DeletePartDetailAsync(PartDetails partProblem);
    }
}
