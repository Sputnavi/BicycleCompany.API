using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IPartService
    {
        Task<List<PartForReadModel>> GetPartListAsync(PartParameters partParameters, HttpResponse response);
        Task<PartForReadModel> GetPartAsync(Guid id);
        Task<Guid> CreatePartAsync(PartForCreateOrUpdateModel model);
        Task UpdatePartAsync(Guid id, PartForCreateOrUpdateModel model);
        Task DeletePartAsync(Guid id);
        Task<PartForCreateOrUpdateModel> GetPartForUpdateModelAsync(Guid id);
    }
}
