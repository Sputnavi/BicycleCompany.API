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

        public async Task<List<BicycleForReadModel>> GetBicycleListAsync(BicycleParameters bicycleParameters, HttpResponse response = null)
        {
            var bicycles = await _bicycleRepository.GetBicycleListAsync(bicycleParameters);

            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(bicycles.MetaData)); 
            }

            return _mapper.Map<List<BicycleForReadModel>>(bicycles);
        }

        public async Task<BicycleForReadModel> GetBicycleAsync(Guid id)
        {
            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            CheckIfFound(id, bicycleEntity);
            return _mapper.Map<BicycleForReadModel>(bicycleEntity);
        }

        public async Task<Guid> CreateBicycleAsync(BicycleForCreateOrUpdateModel model)
        {
            await CheckIfAlreadyExists(model);

            var bicycleEntity = _mapper.Map<Bicycle>(model);

            await _bicycleRepository.CreateBicycleAsync(bicycleEntity);

            return bicycleEntity.Id;
        }

        public async Task DeleteBicycleAsync(Guid id)
        {
            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            CheckIfFound(id, bicycleEntity);

            await _bicycleRepository.DeleteBicycleAsync(bicycleEntity);
        }

        public async Task UpdateBicycleAsync(Guid id, BicycleForCreateOrUpdateModel model)
        {
            await CheckIfAlreadyExists(model);

            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            CheckIfFound(id, bicycleEntity);

            _mapper.Map(model, bicycleEntity);
            await _bicycleRepository.UpdateBicycleAsync(bicycleEntity);
        }

        public async Task<BicycleForCreateOrUpdateModel> GetBicycleForUpdateModelAsync(Guid id)
        {
            var bicycleEntity = await GetBicycleAsync(id);

            return _mapper.Map<BicycleForCreateOrUpdateModel>(bicycleEntity);
        }

        private void CheckIfFound(Guid id, Bicycle bicycleEntity)
        {
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                throw new EntityNotFoundException("Bicycle", id);
            }
        }

        private async Task CheckIfAlreadyExists(BicycleForCreateOrUpdateModel model)
        {
            var bicycle = await _bicycleRepository.GetBicycleByNameAndModelAsync(model.Name, model.Model);
            if (bicycle != null)
            {
                _logger.LogInfo("Bicycle with the same name and model already exists.");
                throw new ArgumentException("Bicycle with the same name and model already exists.");
            }
        }
    }
}
