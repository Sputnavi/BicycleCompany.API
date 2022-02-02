using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions.Utils;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BicycleCompany.DAL.Repository.Extensions
{
    public static class ProblemRepositoryExtensions
    {
        public static IQueryable<Problem> Search(this IQueryable<Problem> problems, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return problems;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return problems.Where(c => c.Description.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Problem> Sort(this IQueryable<Problem> problems, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return problems.OrderBy(c => c.Stage);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Problem>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return problems.OrderBy(c => c.Stage);
            }

            return problems.OrderBy(orderQuery);
        }
    }
}
