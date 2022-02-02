using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request.RequestFeatures;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.DAL.Contracts
{
    public interface IPartRepository
    {
        Task<PagedList<Part>> GetPartsAsync(PartParameters partParameters);
        Task<Part> GetPartAsync(Guid id);
        Task<Part> GetPartByNameAsync(string name);
        Task CreatePartAsync(Part part);
        Task DeletePartAsync(Part part);
        Task UpdatePartAsync(Part part);
    }
}
