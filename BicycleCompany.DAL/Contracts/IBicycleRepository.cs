using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IBicycleRepository
    {
        Task<IEnumerable<Bicycle>> GetBicyclesAsync(bool trackChanges);
        Task<Bicycle> GetBicycleAsync(Guid id, bool trackChanges);
        Task CreateBicycleAsync(Bicycle bicycle);
        Task DeleteBicycleAsync(Bicycle bicycle);
        Task UpdateBicycleAsync(Bicycle bicycle);
    }
}
