using AutoMapper;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.BLL.Utils;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        private readonly IBicycleService _bicycleService;
        private readonly IProblemRepository _problemRepository;
        private readonly IPartService _partService;

        public ClientService(IPartService partService, IBicycleService bicycleService, IClientRepository clientRepository, IProblemRepository problemRepository, ILoggerManager logger, IMapper mapper)
        {
            _partService = partService;
            _bicycleService = bicycleService;
            _clientRepository = clientRepository;
            _problemRepository = problemRepository;
            _logger = logger;
            _mapper = mapper;
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

        public async Task<ClientForReadModel> GetClientAsync(Guid id, ClaimsPrincipal user = null)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            CheckIfFound(id, clientEntity);
            if (user != null && user.IsInRole("User") && clientEntity.UserId != Guid.Parse(user.Identity.Name))
            {
                _logger.LogInfo("You don't have permission to access");
                throw new ForbiddenException();
            }
            return _mapper.Map<ClientForReadModel>(clientEntity);
        }

        public async Task<Guid> CreateClientAsync(ClientForCreateOrUpdateModel model)
        {
            // Check if bicycles and parts exist.
            foreach (var problem in model?.Problems)
            {
                await _bicycleService.GetBicycleAsync(problem.BicycleId);
                foreach (var part in problem?.Parts)
                {
                    await _partService.GetPartAsync(part.PartId);
                }
            }

            var clientEntity = _mapper.Map<Client>(model);

            await _clientRepository.CreateClientAsync(clientEntity);

            return clientEntity.Id;
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            CheckIfFound(id, clientEntity);
            
            await _clientRepository.DeleteClientAsync(clientEntity);
        }

        public async Task UpdateClientAsync(Guid id, ClientForCreateOrUpdateModel model)
        {
            var clientEntity = await _clientRepository.GetClientAsync(id);
            CheckIfFound(id, clientEntity);

            _mapper.Map(model, clientEntity);
            await _clientRepository.UpdateClientAsync(clientEntity);
        }

        public async Task<ClientForCreateOrUpdateModel> GetClientForUpdateModelAsync(Guid id)
        {
            var clientEntity = await GetClientAsync(id);

            return _mapper.Map<ClientForCreateOrUpdateModel>(clientEntity);
        }

        private void CheckIfFound(Guid id, Client clientEntity)
        {
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                throw new EntityNotFoundException("Client", id);
            }
        }

        public async Task<List<ProblemForReadModel>> GetProblemListForClientAsync(Guid clientId, ProblemParameters problemParameters, ClaimsPrincipal user, HttpResponse response = null)
        {
            await GetClientAsync(clientId, user);
            var problems = await _problemRepository.GetProblemListForClientAsync(clientId, problemParameters);

            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(problems.MetaData));
            }

            return _mapper.Map<List<ProblemForReadModel>>(problems);
        }

        public async Task<ProblemForReadModel> GetProblemForClientAsync(Guid clientId, Guid problemId, ClaimsPrincipal user)
        {
            await GetClientAsync(clientId, user);

            var problemEntity = await _problemRepository.GetProblemForClientAsync(clientId, problemId);
            CheckIfFound(problemId, problemEntity);
            return _mapper.Map<ProblemForReadModel>(problemEntity);
        }

        public async Task<Guid> CreateProblemForClientAsync(Guid clientId, ProblemForCreateModel model)
        {
            await GetClientAsync(clientId);
            model.ClientId = clientId;

            var problemEntity = _mapper.Map<Problem>(model);

            await _problemRepository.CreateProblemAsync(problemEntity);

            return problemEntity.Id;
        }

        public async Task DeleteProblemForClientAsync(Guid clientId, Guid problemId)
        {
            await GetClientAsync(clientId);

            var problemEntity = await _problemRepository.GetProblemForClientAsync(clientId, problemId);
            CheckIfFound(problemId, problemEntity);

            await _problemRepository.DeleteProblemAsync(problemEntity);
        }

        private void CheckIfFound(Guid id, Problem problemEntity)
        {
            if (problemEntity is null)
            {
                _logger.LogInfo($"Problem with id: {id} doesn't exist in the database.");
                throw new EntityNotFoundException("Problem", id);
            }
        }
    }
}
