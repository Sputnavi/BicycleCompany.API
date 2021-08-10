using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IBicycleRepository
    {
        Task<PagedList<Bicycle>> GetBicycleListAsync(BicycleParameters bicycleParameters);
        Task<Bicycle> GetBicycleAsync(Guid id);
        Task<Bicycle> GetBicycleByNameAndModelAsync(string name, string model);
        Task CreateBicycleAsync(Bicycle bicycle);
        Task DeleteBicycleAsync(Bicycle bicycle);
        Task UpdateBicycleAsync(Bicycle bicycle);
    }
}
