using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client> GetClientAsync(Guid id);
        Task CreateClientAsync(Client client);
        Task DeleteClientAsync(Client client);
        Task UpdateClientAsync(Client client);
    }
}
