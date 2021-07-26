using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IProblemService
    {
        Task<List<ProblemForReadModel>> GetProblemListAsync(Guid clientId, ProblemParameters problemParameters, HttpResponse response);
        Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid id);
        Task<Guid> CreateProblemAsync(Guid clientId, ProblemForCreateModel model);
        Task UpdateProblemAsync(Guid clientId, Guid id, ProblemForUpdateModel model);
        Task DeleteProblemAsync(Guid clientId, Guid id);
        Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid clientId, Guid id);
        Task<List<PartProblemForReadModel>> GetPartListForProblemAsync(Guid clientId, Guid problemId);
        Task<Guid> CreatePartForProblemAsync(Guid clientId, Guid problemId, PartProblemForCreateModel partProblem);
        Task DeletePartForProblemAsync(Guid clientId, Guid problemId, Guid id);
    }
}
