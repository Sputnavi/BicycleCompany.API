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
    /// <summary>
    /// Service to manage bicycles.
    /// </summary>
    public class BicycleService : IBicycleService
    {
        private readonly IMapper _mapper;
        private readonly IBicycleRepository _bicycleRepository;
        private readonly ILoggerManager _logger;

        public BicycleService(IMapper mapper, IBicycleRepository bicycleRepository, ILoggerManager logger)
        {
            _mapper = mapper;
            _bicycleRepository = bicycleRepository;
            _logger = logger;
        }

        public async Task<List<BicycleForReadModel>> GetBicycleListAsync()
        {
            var bicycles = await _bicycleRepository.GetBicyclesAsync();
            return _mapper.Map<List<BicycleForReadModel>>(bicycles);
        }

        public async Task<BicycleForReadModel> GetBicycleAsync(Guid id)
        {
            var bicycle = await _bicycleRepository.GetBicycleAsync(id);
            if (bicycle is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Bicycle with id: {id} cannot be found!");
            }
            return _mapper.Map<BicycleForReadModel>(bicycle);
        }

        public async Task<Guid> CreateBicycleAsync(BicycleForCreateOrUpdateModel model)
        {
            var bicycleEntity = _mapper.Map<Bicycle>(model);

            await _bicycleRepository.CreateBicycleAsync(bicycleEntity);

            return bicycleEntity.Id;
        }

        public async Task DeleteBicycleAsync(Guid id)
        {
            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Bicycle with id: {id} cannot be found!");
            }

            await _bicycleRepository.DeleteBicycleAsync(bicycleEntity);
        }

        public async Task UpdateBicycleAsync(Guid id, BicycleForCreateOrUpdateModel model)
        {
            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException($"Bicycle with id: {id} cannot be found!");
            }

            _mapper.Map(model, bicycleEntity);
            await _bicycleRepository.UpdateBicycleAsync(bicycleEntity);
        }

        public async Task<BicycleForCreateOrUpdateModel> GetBicycleForUpdateModelAsync(Guid id)
        {
            var bicycleEntity = await GetBicycleAsync(id);

            return _mapper.Map<BicycleForCreateOrUpdateModel>(bicycleEntity);
        }
    }
}
