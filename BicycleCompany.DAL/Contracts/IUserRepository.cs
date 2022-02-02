using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IUserRepository
    {
        Task<PagedList<User>> GetUserListAsync(UserParameters userParameters);
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserByLoginAsync(string login);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
