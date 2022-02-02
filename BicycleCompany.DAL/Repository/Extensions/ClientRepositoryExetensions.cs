using BicycleCompany.DAL.Models;
using BicycleCompany.DAL.Repository.Extensions.Utils;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BicycleCompany.DAL.Repository.Extensions
{
    public static class ClientRepositoryExetensions
    {
        public static IQueryable<Client> Search(this IQueryable<Client> clients, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return clients;
            }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return clients.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Client> Sort(this IQueryable<Client> clients, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return clients.OrderBy(c => c.Name);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Client>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                return clients.OrderBy(c => c.Name);
            }

            return clients.OrderBy(orderQuery);
        }
    }
}
