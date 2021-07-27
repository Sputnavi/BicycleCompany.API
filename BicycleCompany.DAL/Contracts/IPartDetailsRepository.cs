using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartDetailsRepository
    {
        Task<IEnumerable<PartDetails>> GetPartDetailListAsync(Guid problemId);
        Task<PartDetails> GetPartDetailAsync(Guid problemId, Guid partId);
        Task CreatePartDetailAsync(PartDetails partProblem);
        Task DeletePartDetailAsync(PartDetails partProblem);
    }
}
