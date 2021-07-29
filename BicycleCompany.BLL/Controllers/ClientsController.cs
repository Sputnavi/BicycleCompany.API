using BicycleCompany.BLL.Extensions;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IClientService _clientService;

        public ClientsController(ILoggerManager logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        /// <summary>
        /// Return a list of all Clients.
        /// </summary>
        /// <response code="200">List of clients returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetClientList([FromQuery]ClientParameters clientParameters)
        {
            var clients = await _clientService.GetClientListAsync(clientParameters, Response);

            return Ok(clients);
        }

        /// <summary>
        /// Return Client.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <response code="200">Client returned successfully</response> 
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}", Name = "GetClient")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var clientEntity = await _clientService.GetClientAsync(id);

            return Ok(clientEntity);
        }

        /// <summary>
        /// Create new Client.
        /// </summary>
        /// <param name="client">The client object for creation</param>
        /// <response code="201">Client created successfully</response> 
        /// <response code="400">Client model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientForCreateOrUpdateModel client)
        {
            this.ValidateObject();

            var clientId = await _clientService.CreateClientAsync(client);

            return CreatedAtRoute("GetClient", new { id = clientId }, new AddedResponse(clientId));
        }

        /// <summary>
        /// Delete Client.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <response code="204">Client deleted successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            await _clientService.DeleteClientAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update Client information.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <param name="client">The client object which is used for update client with provided id</param>
        /// <response code="204">Client updated successfully</response>
        /// <response code="400">Client model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientForCreateOrUpdateModel client)
        {
            this.ValidateObject();

            await _clientService.UpdateClientAsync(id, client);

            return NoContent();
        }

        /// <summary>
        /// Partially update Client information.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <param name="patchDoc">The document with an array of operations for client with provided id</param>
        /// <response code="204">Client updated successfully</response>
        /// <response code="400">Client model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateClient(Guid id, 
            [FromBody] JsonPatchDocument<ClientForCreateOrUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("Sent patch document is empty.");
            }

            var clientToPatch = await _clientService.GetClientForUpdateModelAsync(id);

            patchDoc.ApplyTo(clientToPatch, ModelState);

            TryValidateModel(clientToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _clientService.UpdateClientAsync(id, clientToPatch);

            return NoContent();
        }

        /// <summary>
        /// Return a list of all Problems for Client.
        /// </summary>
        /// <param name="clientId">The value that is used to find client who has problems</param>
        /// <response code="200">List of problems returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{clientId}/problems")]
        [HttpHead("{clientId}/problems")]
        public async Task<IActionResult> GetProblemListForClient(Guid clientId, [FromQuery] ProblemParameters problemParameters)
        {
            var problems = await _clientService.GetProblemListForClientAsync(clientId, problemParameters, Response);

            return Ok(problems);
        }

        /// <summary>
        /// Return Problem for Client.
        /// </summary>
        /// <param name="clientId">The value that is used to find client who has a problem</param>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <response code="200">Problem returned successfully</response> 
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{clientId}/problems/{problemId}", Name = "GetProblemForClient")]
        public async Task<IActionResult> GetProblemForClient(Guid clientId, Guid problemId)
        {
            var problemEntity = await _clientService.GetProblemForClientAsync(clientId, problemId);

            return Ok(problemEntity);
        }


        /// <summary>
        /// Create new Problem for Client.
        /// </summary>
        /// <param name="clientId">The value that is used to find client who got a problem</param>
        /// <param name="problem">The problem object for creation</param>
        /// <response code="201">Problem created successfully</response> 
        /// <response code="400">Problem model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{clientId}/problems")]
        public async Task<IActionResult> CreateProblemForClient(Guid clientId, [FromBody] ProblemForCreateModel problem)
        {
            this.ValidateObject();

            var problemId = await _clientService.CreateProblemForClientAsync(clientId, problem);

            return CreatedAtRoute("GetProblemForClient", new { clientId = clientId, problemId = problemId }, new AddedResponse(problemId));
        }

        /// <summary>
        /// Delete Problem for Client.
        /// </summary>
        /// <param name="clientId">The value that is used to find client whose problem should be deleted</param>
        /// <param name="problemId">The value that is used to find problem</param>
        /// <response code="204">Problem deleted successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Problem with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{clientId}/problems/{problemId}")]
        public async Task<IActionResult> DeleteProblemForClient(Guid clientId, Guid problemId)
        {
            await _clientService.DeleteProblemForClientAsync(clientId, problemId);

            return NoContent();
        }
    }
}
