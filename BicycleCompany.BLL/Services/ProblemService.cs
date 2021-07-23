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

        public ProblemService(IMapper mapper, IProblemRepository problemRepository, IBicycleRepository bicycleRepository, ILoggerManager logger, IPartProblemRepository partProblemRepository)
        {
            _mapper = mapper;
            _problemRepository = problemRepository;
            _bicycleRepository = bicycleRepository;
            _logger = logger;
            _partProblemRepository = partProblemRepository;
        }

        public async Task<List<ProblemForReadModel>> GetProblemsListAsync(Guid clientId)
        {
            var problems = await _problemRepository.GetProblemsAsync(clientId);
            return _mapper.Map<IEnumerable<Problem>, List<ProblemForReadModel>>(problems);
        }
        
        public async Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid id)
        {
            var problem = await _problemRepository.GetProblemAsync(clientId, id);
            //if (problem is null)
            //{
            //    _logger.LogInfo($"Problem with id: {id} doesn't exist in the database.");
            //}
            return _mapper.Map<Problem, ProblemForReadModel>(problem);
        }

        public async Task<ProblemForReadModel> CreateProblemAsync(Guid clientId, ProblemForCreateModel model)
        {
            var problemEntity = _mapper.Map<Problem>(model);

            await _problemRepository.CreateProblemAsync(clientId, problemEntity);

            //var bicycleEntity = await _bicycleRepository.GetBicycleAsync(model.BicycleId);
            //problemEntity.Bicycle = bicycleEntity;
            return _mapper.Map<ProblemForReadModel>(problemEntity);
        }

        public async Task<Problem> DeleteProblemAsync(Guid clientId, Guid id)
        {
            var problemEntity = await _problemRepository.GetProblemAsync(clientId, id);
            if (problemEntity != null)
            {
                await _problemRepository.DeleteProblemAsync(problemEntity);
            }

            return problemEntity;
        }

        public async Task<Problem> UpdateProblemAsync(Guid clientId, Guid id, ProblemForUpdateModel model)
        {
            var problemEntity = await _problemRepository.GetProblemAsync(clientId, id);
            if (problemEntity != null)
            {
                _mapper.Map(model, problemEntity);
                await _problemRepository.UpdateProblemAsync(problemEntity);
            }

            return problemEntity;
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
