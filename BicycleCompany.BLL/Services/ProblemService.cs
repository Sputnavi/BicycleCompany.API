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
        private readonly IPartDetailsRepository _partDetailsRepository;
        private readonly ILoggerManager _logger;
        private readonly IClientService _clientService;
        private readonly IBicycleService _bicycleService;
        private readonly IPartService _partService;

        public ProblemService(IProblemRepository problemRepository, IPartDetailsRepository partDetailsRepository, IClientService clientService, IBicycleService bicycleService, IMapper mapper, ILoggerManager logger, IPartService partService)
        {
            _problemRepository = problemRepository;
            _partDetailsRepository = partDetailsRepository;
            _clientService = clientService;
            _bicycleService = bicycleService;
            _mapper = mapper;
            _logger = logger;
            _partService = partService;
        }

        public async Task<List<ProblemForReadModel>> GetProblemListAsync(ProblemParameters problemParameters, HttpResponse response)
        {
            var problems = await _problemRepository.GetProblemListAsync(problemParameters);
            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(problems.MetaData));
            }

            return _mapper.Map<List<ProblemForReadModel>>(problems);
        }
        
        public async Task<ProblemForReadModel> GetProblemAsync(Guid problemId)
        {
            var problemEntity = await _problemRepository.GetProblemAsync(problemId);
            CheckIfFound(problemId, problemEntity);
            return _mapper.Map<ProblemForReadModel>(problemEntity);
        }

        public async Task<Guid> CreateProblemAsync(ProblemForCreateModel model)
        {
            await _bicycleService.GetBicycleAsync(model.BicycleId);
            await _clientService.GetClientAsync(model.ClientId);

            if (model.Parts != null)
            {
                foreach (var part in model.Parts)
                {
                    await _partService.GetPartAsync(part.PartId);
                } 
            }

            var problemEntity = _mapper.Map<Problem>(model);

            await _problemRepository.CreateProblemAsync(problemEntity);

            return problemEntity.Id;
        }

        public async Task DeleteProblemAsync(Guid problemId)
        {
            var problemEntity = await _problemRepository.GetProblemAsync(problemId);
            CheckIfFound(problemId, problemEntity);

            await _problemRepository.DeleteProblemAsync(problemEntity);
        }

        public async Task UpdateProblemAsync(Guid id, ProblemForUpdateModel model)
        {
            await _bicycleService.GetBicycleAsync(model.BicycleId);

            var problemEntity = await _problemRepository.GetProblemAsync(id);
            CheckIfFound(id, problemEntity);

            // For succesfully update bicycle.
            problemEntity.Bicycle = null;

            _mapper.Map(model, problemEntity);
            await _problemRepository.UpdateProblemAsync(problemEntity);
        }

        public async Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid problemId)
        {
            var problemEntity = await GetProblemAsync(problemId);

            return _mapper.Map<ProblemForUpdateModel>(problemEntity);
        }

        public async Task<List<PartDetailsForReadModel>> GetPartListForProblemAsync(Guid problemId)
        {
            await GetProblemAsync(problemId);

            var parts = await _partDetailsRepository.GetPartDetailListAsync(problemId);

            return _mapper.Map<List<PartDetailsForReadModel>>(parts);
        }

        public async Task<PartDetailsForReadModel> GetPartForProblemAsync(Guid problemId, Guid partId)
        {
            await GetProblemAsync(problemId);

            var part = await _partDetailsRepository.GetPartDetailAsync(problemId, partId);
            if (part is null)
            {
                _logger.LogInfo($"Part with id: {partId} doesn't exist for problem.");
                throw new EntityNotFoundException("Part", partId);
            }

            return _mapper.Map<PartDetailsForReadModel>(part);
        }

        public async Task<Guid> CreatePartForProblemAsync(Guid problemId, PartDetailsForCreateModel partDetails)
        {
            var problemEntity = await GetProblemAsync(problemId);
            if (problemEntity.Parts.Any(pp => pp.Part.Id.Equals(partDetails.PartId))) 
            {
                _logger.LogInfo("Same part for provided problem already exists!");
                throw new ArgumentException($"Same part for provided problem already exists!");
            }

            var partDetailsEntity = _mapper.Map<PartDetails>(partDetails);

            partDetailsEntity.ProblemId = problemId;
            await _partDetailsRepository.CreatePartDetailAsync(partDetailsEntity);

            return partDetailsEntity.PartId;
        }

        public async Task DeletePartForProblemAsync(Guid problemId, Guid partId)
        {
            await GetProblemAsync(problemId);

            var partDetailsEntity = await _partDetailsRepository.GetPartDetailAsync(problemId, partId);
            if (partDetailsEntity is null)
            {
                _logger.LogInfo($"Part with id: {partId} doesn't exist for problem.");
                throw new EntityNotFoundException("Part", partId);
            }

            await _partDetailsRepository.DeletePartDetailAsync(partDetailsEntity);
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
