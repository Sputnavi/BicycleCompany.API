using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions.Utils;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BicycleCompany.DAL.Repository.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> Search(this IQueryable<User> users, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return users;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return users.Where(c => c.Login.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return users.OrderBy(c => c.Login);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return users.OrderBy(c => c.Login);
            }

            return users.OrderBy(orderQuery);
        }
    }
}
