using AutoMapper;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        public BicycleService(IMapper mapper, IBicycleRepository bicycleRepository)
        {
            _mapper = mapper;
            _bicycleRepository = bicycleRepository;
        }

        public async Task<List<BicycleForReadModel>> GetBicyclesListAsync()
        {
            var bicycles = await _bicycleRepository.GetBicyclesAsync();
            return _mapper.Map<List<BicycleForReadModel>>(bicycles);
        }

        public async Task<BicycleForReadModel> GetBicycleAsync(Guid id)
        {
            var bicycle = await _bicycleRepository.GetBicycleAsync(id);
            return _mapper.Map<BicycleForReadModel>(bicycle);
        }

        public async Task<BicycleForReadModel> CreateBicycleAsync(BicycleForCreateOrUpdateModel model)
        {
            var bicycleEntity = _mapper.Map<Bicycle>(model);

            await _bicycleRepository.CreateBicycleAsync(bicycleEntity);

            return _mapper.Map<BicycleForReadModel>(bicycleEntity);
        }

        public async Task<Bicycle> DeleteBicycleAsync(Guid id)
        {
            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            if (bicycleEntity != null)
            {
                await _bicycleRepository.DeleteBicycleAsync(bicycleEntity);
            }

            return bicycleEntity;
        }

        public async Task<Bicycle> UpdateBicycleAsync(Guid id, BicycleForCreateOrUpdateModel model)
        {
            var bicycleEntity = await _bicycleRepository.GetBicycleAsync(id);
            if (bicycleEntity != null)
            {
                _mapper.Map(model, bicycleEntity);
                await _bicycleRepository.UpdateBicycleAsync(bicycleEntity); 
            }

            return bicycleEntity;
        }
    }
}
