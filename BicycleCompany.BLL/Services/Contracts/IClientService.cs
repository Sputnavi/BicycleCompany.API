using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IClientService
    {
        Task<List<ClientForReadModel>> GetClientsListAsync();
        Task<ClientForReadModel> GetClientAsync(Guid id);
        Task<ClientForReadModel> CreateClientAsync(ClientForCreateOrUpdateModel model);
        Task<Client> UpdateClientAsync(Guid id, ClientForCreateOrUpdateModel model);
        Task<Client> DeleteClientAsync(Guid id);
    }
}
