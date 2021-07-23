using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IProblemService
    {
        Task<List<ProblemForReadModel>> GetProblemListAsync(Guid clientId);
        Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid id);
        Task<Guid> CreateProblemAsync(Guid clientId, ProblemForCreateModel model);
        Task UpdateProblemAsync(Guid clientId, Guid id, ProblemForUpdateModel model);
        Task DeleteProblemAsync(Guid clientId, Guid id);
        Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid clientId, Guid id);
        Task<List<PartProblemForReadModel>> GetPartsListForProblemAsync(Guid clientId, Guid id);
    }
}
