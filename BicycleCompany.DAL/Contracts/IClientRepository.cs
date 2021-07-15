using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClientsAsync(bool trackChanges);
        Task<Client> GetClientAsync(Guid id, bool trackChanges);
        void CreateClient(Client client);
        void DeleteClient(Client client);
    }
}
