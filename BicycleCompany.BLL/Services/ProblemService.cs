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
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services
{
    /// <summary>
    /// Service to manage problems.
    /// </summary>
    public class ProblemService : IProblemService
    {
        private readonly IMapper _mapper;
        private readonly IProblemRepository _problemRepository;
        private readonly IPartProblemRepository _partProblemRepository;
        private readonly ILoggerManager _logger;
        private readonly IClientService _clientService;
        private readonly IBicycleService _bicycleService;

        public ProblemService(IMapper mapper, IProblemRepository problemRepository, ILoggerManager logger, IPartProblemRepository partProblemRepository, IClientService clientService, IBicycleService bicycleService)
        {
            _mapper = mapper;
            _problemRepository = problemRepository;
            _logger = logger;
            _partProblemRepository = partProblemRepository;
            _clientService = clientService;
            _bicycleService = bicycleService;
        }

        public async Task<List<ProblemForReadModel>> GetProblemListAsync(Guid clientId, ProblemParameters problemParameters, HttpResponse response)
        {
            await _clientService.GetClientAsync(clientId);

            var problems = await _problemRepository.GetProblemListAsync(clientId, problemParameters);
            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(problems.MetaData));
            }

            return _mapper.Map<List<ProblemForReadModel>>(problems);
        }
        
        public async Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid problemId)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = await _problemRepository.GetProblemAsync(clientId, problemId);
            CheckIfFound(problemId, problemEntity);
            return _mapper.Map<ProblemForReadModel>(problemEntity);
        }

        public async Task<Guid> CreateProblemAsync(Guid clientId, ProblemForCreateModel model)
        {
            await _clientService.GetClientAsync(clientId);
            await _bicycleService.GetBicycleAsync(model.BicycleId);

            var problemEntity = _mapper.Map<Problem>(model);

            await _problemRepository.CreateProblemAsync(clientId, problemEntity);

            return problemEntity.Id;
        }

        public async Task DeleteProblemAsync(Guid clientId, Guid problemId)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = await _problemRepository.GetProblemAsync(clientId, problemId);
            CheckIfFound(problemId, problemEntity);

            await _problemRepository.DeleteProblemAsync(problemEntity);
        }

        public async Task UpdateProblemAsync(Guid clientId, Guid id, ProblemForUpdateModel model)
        {
            await _clientService.GetClientAsync(clientId);
            await _bicycleService.GetBicycleAsync(model.BicycleId);

            var problemEntity = await _problemRepository.GetProblemAsync(clientId, id);
            CheckIfFound(id, problemEntity);

            _mapper.Map(model, problemEntity);
            await _problemRepository.UpdateProblemAsync(problemEntity);
        }
        public async Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid clientId, Guid problemId)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = await GetProblemAsync(clientId, problemId);

            return _mapper.Map<ProblemForUpdateModel>(problemEntity);
        }

        public async Task<List<PartProblemForReadModel>> GetPartListForProblemAsync(Guid clientId, Guid problemId)
        {
            await GetProblemAsync(clientId, problemId);

            var parts = await _partProblemRepository.GetPartProblemsAsync(clientId, problemId);

            return _mapper.Map<List<PartProblemForReadModel>>(parts);
        }

        public async Task<Guid> CreatePartForProblemAsync(Guid clientId, Guid problemId, PartProblemForCreateModel partProblem)
        {
            var problemEntity = await GetProblemAsync(clientId, problemId);
            if (problemEntity.Parts.Any(pp => pp.Part.Id.Equals(partProblem.PartId))) 
            {
                _logger.LogInfo("Same part for provided problem already exists!");
                throw new ArgumentException($"Same part for provided problem already exists!");
            }

            var partProblemEntity = _mapper.Map<PartProblem>(partProblem);

            await _partProblemRepository.CreatePartProblemAsync(clientId, problemId, partProblemEntity);

            return partProblemEntity.Id;
        }

        public async Task DeletePartForProblemAsync(Guid clientId, Guid problemId, Guid partProblemId)
        {
            await GetProblemAsync(clientId, problemId);

            var partProblemEntity = await _partProblemRepository.GetPartProblemAsync(clientId, problemId, partProblemId);
            if (partProblemEntity is null)
            {
                _logger.LogInfo($"Part-Problem with id: {partProblemId} doesn't exist in the database.");
                throw new EntityNotFoundException("Part-Problem", partProblemId);
            }

            await _partProblemRepository.DeletePartProblemAsync(partProblemEntity);
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
