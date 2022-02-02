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
        Task<List<ProblemForReadModel>> GetProblemListAsync(ProblemParameters problemParameters, HttpResponse response);
        Task<ProblemForReadModel> GetProblemAsync(Guid problemId);
        Task<Guid> CreateProblemAsync(ProblemForCreateModel model);
        Task UpdateProblemAsync(Guid problemId, ProblemForUpdateModel model);
        Task DeleteProblemAsync(Guid problemId);
        Task<ProblemForUpdateModel> GetProblemForUpdateModelAsync(Guid problemId);

        Task<List<PartDetailsForReadModel>> GetPartListForProblemAsync(Guid problemId);
        Task<PartDetailsForReadModel> GetPartForProblemAsync(Guid problemId, Guid partId);
        Task<Guid> CreatePartForProblemAsync(Guid problemId, PartDetailsForCreateModel partDetails);
        Task DeletePartForProblemAsync(Guid problemId, Guid partDetailsId);
    }
}
