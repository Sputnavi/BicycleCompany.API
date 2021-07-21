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
        Task<List<PartForReadModel>> GetPartsListAsync();
        Task<PartForReadModel> GetPartAsync(Guid id);
        Task<PartForReadModel> CreatePartAsync(PartForCreateOrUpdateModel model);
        Task<Part> UpdatePartAsync(Guid id, PartForCreateOrUpdateModel model);
        Task<Part> DeletePartAsync(Guid id);
    }
}
