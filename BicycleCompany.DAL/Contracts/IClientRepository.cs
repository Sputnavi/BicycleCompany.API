using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IClientRepository
    {
        Task<PagedList<Client>> GetClientListAsync(ClientParameters clientParameters);
        Task<Client> GetClientAsync(Guid id);
        Task CreateClientAsync(Client client);
        Task DeleteClientAsync(Client client);
        Task UpdateClientAsync(Client client);
    }
}
