using AutoMapper;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services
{
    /// <summary>
    /// Service to manage clients.
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;
        private readonly ILoggerManager _logger;

        public ClientService(IMapper mapper, IClientRepository clientRepository, ILoggerManager logger)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<List<ClientForReadModel>> GetClientListAsync(ClientParameters clientParameters, HttpResponse response = null)
        {
            var clients = await _clientRepository.GetClientListAsync(clientParameters);

            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(clients.MetaData));
            }

            return _mapper.Map<List<ClientForReadModel>>(clients);
        }

        public async Task<ClientForReadModel> GetClientAsync(Guid id)
        {
            var client = await _clientRepository.GetClientAsync(id);
            if (client is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Client with id: {id} cannot be found!");
            }
            return _mapper.Map<ClientForReadModel>(client);
           }

        public async Task<Guid> CreateClientAsync(ClientForCreateOrUpdateModel model)
        {
            var clientEntity = _mapper.Map<Client>(model);

            await _clientRepository.CreateClientAsync(clientEntity);

            return clientEntity.Id;
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Client with id: {id} cannot be found!");
            }
            
            await _clientRepository.DeleteClientAsync(clientEntity);
        }

        public async Task UpdateClientAsync(Guid id, ClientForCreateOrUpdateModel model)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Client with id: {id} cannot be found!");
            }

            _mapper.Map(model, clientEntity);
            await _clientRepository.UpdateClientAsync(clientEntity);
        }

        public async Task<ClientForCreateOrUpdateModel> GetClientForUpdateModelAsync(Guid id)
        {
            var clientEntity = await GetClientAsync(id);

            return _mapper.Map<ClientForCreateOrUpdateModel>(clientEntity);
        }
    }
}
