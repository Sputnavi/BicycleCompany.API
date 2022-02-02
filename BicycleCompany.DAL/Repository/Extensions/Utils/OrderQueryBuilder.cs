using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BicycleCompany.DAL.Repository.Extensions.Utils
{
    /// <summary>
    /// Dynamic builder of order by query.
    /// </summary>
    public static class OrderQueryBuilder
    {
        /// <summary>
        /// Normalize query string for LINQ.
        /// </summary>
        /// <param name="orderByQueryString">
        /// String with sorting fields separated by commas. 
        /// There are shouldn't be spaces after commas. 
        /// There is should be "desc" after field for descending order and nothing for ascending.
        /// </param>
        /// <returns>Prepared string to put inside OrderBy.</returns>
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    continue;
                }

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                {
                    continue;
                }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
