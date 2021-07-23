using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IPartService
    {
        Task<List<PartForReadModel>> GetPartListAsync();
        Task<PartForReadModel> GetPartAsync(Guid id);
        Task<Guid> CreatePartAsync(PartForCreateOrUpdateModel model);
        Task UpdatePartAsync(Guid id, PartForCreateOrUpdateModel model);
        Task DeletePartAsync(Guid id);
        Task<PartForCreateOrUpdateModel> GetPartForUpdateModelAsync(Guid id);
    }
}
