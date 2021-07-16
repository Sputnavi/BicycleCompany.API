using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task CreateClientAsync(Client client) => CreateAsync(client);

        public Task DeleteClientAsync(Client client) => DeleteAsync(client);

        public async Task<Client> GetClientAsync(Guid id, bool trackChanges) => 
            await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Client>> GetClientsAsync(bool trackChanges) => 
            await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
