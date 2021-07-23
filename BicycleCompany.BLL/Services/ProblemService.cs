using AutoMapper;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IBicycleRepository _bicycleRepository;
        private readonly IPartProblemRepository _partProblemRepository;
        private readonly ILoggerManager _logger;
        private readonly IClientService _clientService;

        public ProblemService(IMapper mapper, IProblemRepository problemRepository, IBicycleRepository bicycleRepository, ILoggerManager logger, IPartProblemRepository partProblemRepository, IClientService clientService)
        {
            _mapper = mapper;
            _problemRepository = problemRepository;
            _bicycleRepository = bicycleRepository;
            _logger = logger;
            _partProblemRepository = partProblemRepository;
            _clientService = clientService;
        }

        public async Task<List<ProblemForReadModel>> GetProblemListAsync(Guid clientId)
        {
            await _clientService.GetClientAsync(clientId);

            var problems = await _problemRepository.GetProblemsAsync(clientId);

            return _mapper.Map<IEnumerable<Problem>, List<ProblemForReadModel>>(problems);
        }
        
        public async Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid id)
        {
            await _clientService.GetClientAsync(clientId);

            var problem = await _problemRepository.GetProblemAsync(clientId, id);
            if (problem is null)
            {
                _logger.LogInfo($"Problem with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Problem with id: {id} cannot be found!");
            }
            return _mapper.Map<ProblemForReadModel>(problem);
        }

        public async Task<Guid> CreateProblemAsync(Guid clientId, ProblemForCreateModel model)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = _mapper.Map<Problem>(model);

            await _problemRepository.CreateProblemAsync(clientId, problemEntity);

            return problemEntity.Id;
        }

        public async Task DeleteProblemAsync(Guid clientId, Guid id)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = await _problemRepository.GetProblemAsync(clientId, id);
            if (problemEntity is null)
            {
                _logger.LogInfo($"Problem with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Problem with id: {id} cannot be found!");
            }

            await _problemRepository.DeleteProblemAsync(problemEntity);
        }

        public async Task UpdateProblemAsync(Guid clientId, Guid id, ProblemForUpdateModel model)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = await _problemRepository.GetProblemAsync(clientId, id);
            if (problemEntity is null)
            {
                _logger.LogInfo($"Problem with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Problem with id: {id} cannot be found!");
            }

            _mapper.Map(model, problemEntity);
            await _problemRepository.UpdateProblemAsync(problemEntity);
        }
        public async Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid clientId, Guid id)
        {
            await _clientService.GetClientAsync(clientId);

            var problemEntity = await GetProblemAsync(clientId, id);

            return _mapper.Map<ProblemForUpdateModel>(problemEntity);
        }

        public async Task<List<PartProblemForReadModel>> GetPartsListForProblemAsync(Guid clientId, Guid id)
        {
            /*var parts = await _partProblemRepository.GetParts(clientId, id);
            return _mapper.Map<List<PartProblemForReadModel>>(parts);*/
            
            var problemEntity = await _problemRepository.GetProblemAsync(clientId, id);
            return _mapper.Map<List<PartProblemForReadModel>>(problemEntity.PartProblems);
        }
    }
}
