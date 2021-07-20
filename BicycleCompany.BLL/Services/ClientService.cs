using AutoMapper;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public ClientService(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<List<ClientForReadModel>> GetClientsListAsync()
        {
            var clients = await _clientRepository.GetClientsAsync();
            return _mapper.Map<List<ClientForReadModel>>(clients);
        }

        public async Task<ClientForReadModel> GetClientAsync(Guid id)
        {
            var client = await _clientRepository.GetClientAsync(id);
            return _mapper.Map<ClientForReadModel>(client);
        }

        public async Task<ClientForReadModel> CreateClientAsync(ClientForCreateOrUpdateModel model)
        {
            var clientEntity = _mapper.Map<Client>(model);

            await _clientRepository.CreateClientAsync(clientEntity);

            return _mapper.Map<ClientForReadModel>(clientEntity);
        }

        public async Task<Client> DeleteClientAsync(Guid id)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            if (clientEntity != null)
            {
                await _clientRepository.DeleteClientAsync(clientEntity);
            }

            return clientEntity;
        }

        public async Task<Client> UpdateClientAsync(Guid id, ClientForCreateOrUpdateModel model)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            if (clientEntity != null)
            {
                _mapper.Map(model, clientEntity);
                await _clientRepository.UpdateClientAsync(clientEntity);
            }

            return clientEntity;
        }
    }
}
