using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions;
using BicycleCompany.Models.Request.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
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

        public Task UpdateClientAsync(Client client) => UpdateAsync(client);

        public async Task<Client> GetClientAsync(Guid id) => 
            await FindByCondition(c => c.Id.Equals(id)).SingleOrDefaultAsync();

        public async Task<PagedList<Client>> GetClientListAsync(ClientParameters clientParameters)
        {
            var clients = await FindAll()
                .Search(clientParameters.SearchTerm)
                .Sort(clientParameters.OrderBy)
                .ToListAsync();

            return PagedList<Client>.ToPagedList(clients, clientParameters.PageNumber, clientParameters.PageSize);
        }
    }
}
