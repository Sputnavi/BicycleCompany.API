using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IBicycleService
    {
        Task<List<BicycleForReadModel>> GetBicyclesListAsync();
        Task<BicycleForReadModel> GetBicycleAsync(Guid id);
        Task<BicycleForReadModel> CreateBicycleAsync(BicycleForCreateOrUpdateModel model);
        Task<Bicycle> UpdateBicycleAsync(Guid id, BicycleForCreateOrUpdateModel model);
        Task<Bicycle> DeleteBicycleAsync(Guid id);
    }
}
