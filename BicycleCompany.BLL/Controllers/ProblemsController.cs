using AutoMapper;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
    [Route("api/clients/{clientId}/[controller]")]
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
        /// <param name="clientId">The value that is used to find client who has problems</param>
        /// <response code="200">List of problems returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetProblems(Guid clientId)
        {
            var problems = await _problemService.GetProblemListAsync(clientId);

            return Ok(problems);
        }

        /// <summary>
        /// Return Problem.
        /// </summary>
        /// <param name="clientId">The value that is used to find client who has a problem</param>
        /// <param name="id">The value that is used to find problem</param>
        /// <response code="200">Problem returned successfully</response> 
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}", Name = "GetProblem")]
        public async Task<IActionResult> GetProblem(Guid clientId, Guid id)
        {
            var problemEntity = await _problemService.GetProblemAsync(clientId, id);

            return Ok(problemEntity);
        }

        /// <summary>
        /// Create new Problem.
        /// </summary>
        /// /// <param name="clientId">The value that is used to find client who got a problem</param>
        /// <param name="problem">The problem object for creation</param>
        /// <response code="201">Problem created successfully</response> 
        /// <response code="400">Problem model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateProblem(Guid clientId, [FromBody] ProblemForCreateModel problem)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException(string.Join(", ", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage)));
            }

            var problemId = await _problemService.CreateProblemAsync(clientId, problem);

            return Created($"api/clients/{clientId}/problems/" + problemId, new AddedResponse(problemId));
        }

        /// <summary>
        /// Delete Problem.
        /// </summary>
        /// <param name="clientId">The value that is used to find client whose problem should be deleted</param>
        /// <param name="id">The value that is used to find problem</param>
        /// <response code="204">Problem deleted successfully</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProblem(Guid clientId, Guid id)
        {
            await _problemService.DeleteProblemAsync(clientId, id);

            return NoContent();
        }

        /// <summary>
        /// Update Problem information.
        /// </summary>
        /// <param name="clientId">The value that is used to find client whose problem should be updated</param>
        /// <param name="id">The value that is used to find problem</param>
        /// <param name="problem">The problem object which is used for update problem with provided id</param>
        /// <response code="204">Problem deleted successfully</response>
        /// <response code="400">Problem model is invalid</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProblem(Guid clientId, Guid id, [FromBody] ProblemForUpdateModel problem)
        {
            if (!ModelState.IsValid)
            {
                throw new ArgumentException(string.Join(", ", ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage)));
            }

            await _problemService.UpdateProblemAsync(clientId, id, problem);

            return NoContent();
        }

        /// <summary>
        /// Partially update Problem information.
        /// </summary>
        /// <param name="clientId">The value that is used to find client whose problem should be partially updated</param>
        /// <param name="id">The value that is used to find problem</param>
        /// <param name="patchDoc">The document with an array of operations for problem with provided id</param>
        /// <response code="204">Problem deleted successfully</response>
        /// <response code="400">Problem model is invalid</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateProblem(Guid clientId, Guid id,
            [FromBody] JsonPatchDocument<ProblemForUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from problem is null.");
                return BadRequest("patchDoc object is null");
            }

            var problemToPatch = await _problemService.GetProblemForUpdateModelAsync(clientId, id);

            patchDoc.ApplyTo(problemToPatch, ModelState);

            TryValidateModel(problemToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _problemService.UpdateProblemAsync(clientId, id, problemToPatch);

            return NoContent();
        }
        /*
        [HttpGet("{id}/parts")]
        public async Task<IActionResult> GetPartsForProblem(Guid clientId, Guid id)
        {
            var parts = await _problemService.GetPartsListForProblemAsync(clientId, id);

            return Ok(parts);
        }
        
        [HttpPost("{id}/parts")]
        public async Task<IActionResult> CreatePartForProblem(Guid clientId, Guid id, [FromBody] PartProblemForCreateModel)
        {

            //var problemToReturn = await _problemService.CreateProblemAsync(clientId, problem);

            //return CreatedAtRoute("GetProblem", new { clientId, id = problemToReturn.Id }, problemToReturn);
        }*/
    }
}