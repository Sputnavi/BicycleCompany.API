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
        Task<ProblemForReadModel> GetProblemAsync(Guid clientId, Guid problemId);
        Task<Guid> CreateProblemAsync(Guid clientId, ProblemForCreateModel model);
        Task UpdateProblemAsync(Guid clientId, Guid problemId, ProblemForUpdateModel model);
        Task DeleteProblemAsync(Guid clientId, Guid problemId);
        Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid clientId, Guid problemId);
        Task<List<PartDetailsForReadModel>> GetPartListForProblemAsync(Guid clientId, Guid problemId);
        Task<Guid> CreatePartForProblemAsync(Guid clientId, Guid problemId, PartDetailsForCreateModel partProblem);
        Task DeletePartForProblemAsync(Guid clientId, Guid problemId, Guid partProblemId);
    }
}
