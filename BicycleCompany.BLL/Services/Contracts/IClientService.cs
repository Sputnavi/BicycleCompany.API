using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IClientService
    {
        Task<List<ClientForReadModel>> GetClientListAsync(ClientParameters clientParameters, HttpResponse response);
        Task<ClientForReadModel> GetClientAsync(Guid id);
        Task<Guid> CreateClientAsync(ClientForCreateOrUpdateModel model);
        Task UpdateClientAsync(Guid id, ClientForCreateOrUpdateModel model);
        Task DeleteClientAsync(Guid id);
        Task<ClientForCreateOrUpdateModel> GetClientForUpdateModelAsync(Guid id);
    }
}
