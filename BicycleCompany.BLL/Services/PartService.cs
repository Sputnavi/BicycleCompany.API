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
    /// Service to manage parts.
    /// </summary>
    public class PartService : IPartService
    {
        private readonly IMapper _mapper;
        private readonly IPartRepository _partRepository;

        public PartService(IMapper mapper, IPartRepository partRepository)
        {
            _mapper = mapper;
            _partRepository = partRepository;
        }

        public async Task<List<PartForReadModel>> GetPartsListAsync()
        {
            var parts = await _partRepository.GetPartsAsync();
            return _mapper.Map<List<PartForReadModel>>(parts);
        }

        public async Task<PartForReadModel> GetPartAsync(Guid id)
        {
            var part = await _partRepository.GetPartAsync(id);
            return _mapper.Map<PartForReadModel>(part);
        }

        public async Task<PartForReadModel> CreatePartAsync(PartForCreateOrUpdateModel model)
        {
            var partEntity = _mapper.Map<Part>(model);

            await _partRepository.CreatePartAsync(partEntity);

            return _mapper.Map<PartForReadModel>(partEntity);
        }

        public async Task<Part> DeletePartAsync(Guid id)
        {
            var partEntity = await _partRepository.GetPartAsync(id);
            if (partEntity != null)
            {
                await _partRepository.DeletePartAsync(partEntity);
            }

            return partEntity;
        }

        public async Task<Part> UpdatePartAsync(Guid id, PartForCreateOrUpdateModel model)
        {
            var partEntity = await _partRepository.GetPartAsync(id);
            if (partEntity != null)
            {
                _mapper.Map(model, partEntity);
                await _partRepository.UpdatePartAsync(partEntity);
            }

            return partEntity;
        }

    }
}
