using BicycleCompany.BLL.Extensions;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
    [Authorize(Roles = "Manager, Master")]
    [SwaggerTag("Manager, Master")]
    [Route("api/problems")]
    [ApiController]
    public class ProblemsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IProblemService _problemService;

        public ProblemsController(ILoggerManager logger, IProblemService problemService)
        {
            _logger = logger;
            _problemService = problemService;
        }

        /// <summary>
        /// Return a list of all Problems.
        /// </summary>
        /// <response code="200">List of problems returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetProblemList([FromQuery] ProblemParameters problemParameters)
        {
            var problems = await _problemService.GetProblemListAsync(problemParameters, Response);

            return Ok(problems);
        }

        /// <summary>
        /// Return Problem.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <response code="200">Problem returned successfully</response> 
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpGet("{problemId}", Name = "GetProblem")]
        public async Task<IActionResult> GetProblem(Guid problemId)
        {
            var problemEntity = await _problemService.GetProblemAsync(problemId);

            return Ok(problemEntity);
        }

        /// <summary>
        /// Create new Problem.
        /// </summary>
        /// <param name="problem">The problem object for creation</param>
        /// <response code="201">Problem created successfully</response> 
        /// <response code="400">Problem model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPost]
        public async Task<IActionResult> CreateProblem([FromBody] ProblemForCreateModel problem)
        {
            this.ValidateObject();

            var problemId = await _problemService.CreateProblemAsync(problem);

            return CreatedAtRoute("GetProblem", new { problemId = problemId }, new AddedResponse(problemId));
        }

        /// <summary>
        /// Delete Problem.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <response code="204">Problem deleted successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpDelete("{problemId}")]
        public async Task<IActionResult> DeleteProblem(Guid problemId)
        {
            await _problemService.DeleteProblemAsync(problemId);

            return NoContent();
        }

        /// <summary>
        /// Update Problem information.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <param name="problem">The problem object which is used for update problem with provided id</param>
        /// <response code="204">Problem updated successfully</response>
        /// <response code="400">Problem model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPut("{problemId}")]
        public async Task<IActionResult> UpdateProblem(Guid problemId, [FromBody] ProblemForUpdateModel problem)
        {
            this.ValidateObject();

            await _problemService.UpdateProblemAsync(problemId, problem);

            return NoContent();
        }

        /// <summary>
        /// Partially update Problem information.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <param name="patchDoc">The document with an array of operations for problem with provided id</param>
        /// <response code="204">Problem updated successfully</response>
        /// <response code="400">Problem model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPatch("{problemId}")]
        public async Task<IActionResult> PartiallyUpdateProblem(Guid problemId,
            [FromBody] JsonPatchDocument<ProblemForUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from problem is null.");
                return BadRequest("Sent patch document is empty.");
            }

            var problemToPatch = await _problemService.GetProblemForUpdateModelAsync(problemId);

            patchDoc.ApplyTo(problemToPatch, ModelState);

            TryValidateModel(problemToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _problemService.UpdateProblemAsync(problemId, problemToPatch);

            return NoContent();
        }

        /// <summary>
        /// Return a list of all Parts for Problem.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <response code="200">List of parts returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpGet("{problemId}/parts")]
        public async Task<IActionResult> GetPartListForProblem(Guid problemId)
        {
            var parts = await _problemService.GetPartListForProblemAsync(problemId);

            return Ok(parts);
        }

        /// <summary>
        /// Return Part for Problem.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <param name="partId">The value that is used to find part</param>
        /// <response code="200">Part returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpGet("{problemId}/parts/{partId}", Name = "GetPartForProblem")]
        public async Task<IActionResult> GetPartForProblem(Guid problemId, Guid partId)
        {
            var part = await _problemService.GetPartForProblemAsync(problemId, partId);

            return Ok(part);
        }

        /// <summary>
        /// Add new Part for problem.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <param name="part">The Part details object for adding</param>
        /// <response code="201">Part added to problem successfully</response>
        /// <response code="400">Part details model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPost("{problemId}/parts")]
        public async Task<IActionResult> CreatePartForProblem(Guid problemId, [FromBody] PartDetailsForCreateModel part)
        {
            this.ValidateObject();

            var partDetailsId = await _problemService.CreatePartForProblemAsync(problemId, part);

            return CreatedAtRoute("GetPartForProblem", new AddedResponse(partDetailsId));
        }

        /// <summary>
        /// Delete Part for Problem.
        /// </summary>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <param name="partId">The value that is used to find part</param>
        /// <response code="204">Part deleted for problem successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Part-Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpDelete("{problemId}/parts/{partId}")]
        public async Task<IActionResult> DeletePartForProblem(Guid problemId, Guid partId)
        {
            await _problemService.DeletePartForProblemAsync(problemId, partId);

            return NoContent();
        }
    }
}