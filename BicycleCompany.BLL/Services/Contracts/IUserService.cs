using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IUserService
    {
        Task<List<UserForReadModel>> GetUserListAsync(UserParameters userParameters, HttpResponse response);
        Task<UserForReadModel> GetUserAsync(Guid id);
        Task CreateUserAsync(UserForRegistrationModel userForRegistration);
        Task UpdateUserAsync(Guid id, UserForUpdateModel user);
        Task<UserForUpdateModel> GetUserForUpdateModelAsync(Guid id);
    }
}
