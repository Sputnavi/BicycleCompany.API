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
    /// Service to manage parts.
    /// </summary>
    public class PartService : IPartService
    {
        private readonly IMapper _mapper;
        private readonly IPartRepository _partRepository;
        private readonly ILoggerManager _logger;

        public PartService(IMapper mapper, IPartRepository partRepository, ILoggerManager logger)
        {
            _mapper = mapper;
            _partRepository = partRepository;
            _logger = logger;
        }

        public async Task<List<PartForReadModel>> GetPartListAsync(PartParameters partParameters, HttpResponse response)
        {
            var parts = await _partRepository.GetPartsAsync(partParameters);
            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(parts.MetaData));
            }

            return _mapper.Map<List<PartForReadModel>>(parts);
        }

        public async Task<PartForReadModel> GetPartAsync(Guid id)
        {
            var partEntity = await _partRepository.GetPartAsync(id);
            CheckIfFound(id, partEntity);
            return _mapper.Map<PartForReadModel>(partEntity);
        }

        public async Task<Guid> CreatePartAsync(PartForCreateOrUpdateModel model)
        {
            await CheckIfAlreadyExists(model);

            var partEntity = _mapper.Map<Part>(model);

            await _partRepository.CreatePartAsync(partEntity);

            return partEntity.Id;
        }

        public async Task DeletePartAsync(Guid id)
        {
            var partEntity = await _partRepository.GetPartAsync(id);

            CheckIfFound(id, partEntity);

            await _partRepository.DeletePartAsync(partEntity);
        }

        public async Task UpdatePartAsync(Guid id, PartForCreateOrUpdateModel model)
        {
            await CheckIfAlreadyExists(model);

            var partEntity = await _partRepository.GetPartAsync(id);
            CheckIfFound(id, partEntity);

            _mapper.Map(model, partEntity);
            await _partRepository.UpdatePartAsync(partEntity);
        }

        public async Task<PartForCreateOrUpdateModel> GetPartForUpdateModelAsync(Guid id)
        {
            var partEntity = await GetPartAsync(id);

            return _mapper.Map<PartForCreateOrUpdateModel>(partEntity);
        }

        private void CheckIfFound(Guid id, Part partEntity)
        {
            if (partEntity is null)
            {
                _logger.LogInfo($"Part with id: {id} doesn't exist in the database.");
                throw new EntityNotFoundException("Part", id);
            }
        }

        private async Task CheckIfAlreadyExists(PartForCreateOrUpdateModel model)
        {
            var part = await _partRepository.GetPartByNameAsync(model.Name);
            if (part != null)
            {
                _logger.LogInfo("Part with the same name already exists.");
                throw new ArgumentException("Part with the same name already exists.");
            }
        }
    }
}
