using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions;
using BicycleCompany.Models.Request.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Task<User> GetUserAsync(Guid id) => 
            FindByCondition(u => u.Id.Equals(id)).SingleOrDefaultAsync();

        public Task<User> GetUserByLoginAsync(string login) =>
            FindByCondition(u => u.Login == login).SingleOrDefaultAsync();

        public async Task<PagedList<User>> GetUserListAsync(UserParameters userParameters)
        {
            var users = await FindAll()
                .Search(userParameters.SearchTerm)
                .Sort(userParameters.OrderBy)
                .ToListAsync();

            return PagedList<User>.ToPagedList(users, userParameters.PageNumber, userParameters.PageSize);
        }

        public Task CreateUserAsync(User user) => CreateAsync(user);


        public Task UpdateUserAsync(User user) => UpdateAsync(user);
    }
}
