using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions.Utils;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BicycleCompany.DAL.Repository.Extensions
{
    public static class BicycleRepositoryExtensions
    {
        public static IQueryable<Bicycle> Search(this IQueryable<Bicycle> bicycles, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return bicycles;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return bicycles.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Bicycle> Sort(this IQueryable<Bicycle> bicycles, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return bicycles.OrderBy(c => c.Name);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Bicycle>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return bicycles.OrderBy(c => c.Name);
            }

            return bicycles.OrderBy(orderQuery);
        }
    }
}
