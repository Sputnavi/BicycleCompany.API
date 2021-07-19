using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>> GetPartsAsync();
        Task<Part> GetPartAsync(Guid id);
        Task CreatePartAsync(Part part);
        Task DeletePartAsync(Part part);
        Task UpdatePartAsync(Part part);
    }
}
