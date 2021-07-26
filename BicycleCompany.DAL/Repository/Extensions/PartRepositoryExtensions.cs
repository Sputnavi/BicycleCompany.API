using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions.Utils;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BicycleCompany.DAL.Repository.Extensions
{
    public static class PartRepositoryExtensions
    {
        public static IQueryable<Part> Search(this IQueryable<Part> parts, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return parts;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return parts.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Part> Sort(this IQueryable<Part> parts, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return parts.OrderBy(c => c.Name);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Part>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return parts.OrderBy(c => c.Name);
            }

            return parts.OrderBy(orderQuery);
        }

        public static IQueryable<Part> FilterParts(this IQueryable<Part> parts, int minAmount, int maxAmount) =>
            parts.Where(p => p.Amount >= minAmount && p.Amount <= maxAmount);
    }
}
