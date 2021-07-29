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
    public class UserService : IUserService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(ILoggerManager logger, IMapper mapper, IUserRepository userRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<UserForReadModel>> GetUserListAsync(UserParameters userParameters, HttpResponse response)
        {
            var users = await _userRepository.GetUserListAsync(userParameters);
            if (response != null)
            {
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(users.MetaData));
            }

            return _mapper.Map<List<UserForReadModel>>(users);
        }

        public async Task<UserForReadModel> GetUserAsync(Guid id)
        {
            var userEntity = await _userRepository.GetUserAsync(id);
            CheckIfFound(id, userEntity);
            return _mapper.Map<UserForReadModel>(userEntity);
        }

        public async Task CreateUserAsync(UserForRegistrationModel userForRegistration)
        {
            var userEntity = await _userRepository.GetUserByLoginAsync(userForRegistration.Login);
            if (userEntity != null)
            {
                _logger.LogInfo("User with the same login already exists.");
                throw new ArgumentException("User with the same login already exists.");
            }

            var user = _mapper.Map<User>(userForRegistration);
            await _userRepository.CreateUserAsync(user);
        }


        public async Task UpdateUserAsync(Guid id, UserForUpdateModel user)
        {
            var userEntity = await _userRepository.GetUserAsync(id);
            CheckIfFound(id, userEntity);

            _mapper.Map(user, userEntity);
            await _userRepository.UpdateUserAsync(userEntity);
        }

        public async Task<UserForUpdateModel> GetUserForUpdateModelAsync(Guid id)
        {
            var userEntity = await GetUserAsync(id);

            return _mapper.Map<UserForUpdateModel>(userEntity);
        }

        private void CheckIfFound(Guid id, User userEntity)
        {
            if (userEntity is null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                throw new EntityNotFoundException("User", id);
            }
        }
    }
}
