using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>> GetPartsAsync(bool trackChanges);
        Task<Part> GetPartAsync(Guid id, bool trackChanges);
        Task CreatePartAsync(Part part);
        Task DeletePartAsync(Part part);
        Task UpdatePartAsync(Part part);
    }
}
