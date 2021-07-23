using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IProblemService
    {
        Task<List<ProblemForReadModel>> GetProblemsListAsync(Guid clientId);
        Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid id);
        Task<ProblemForReadModel> CreateProblemAsync(Guid clientId, ProblemForCreateModel model);
        Task<Problem> UpdateProblemAsync(Guid clientId, Guid id, ProblemForUpdateModel model);
        Task<Problem> DeleteProblemAsync(Guid clientId, Guid id);
        Task<List<PartProblemForReadModel>> GetPartsListForProblemAsync(Guid clientId, Guid id);
    }
}
