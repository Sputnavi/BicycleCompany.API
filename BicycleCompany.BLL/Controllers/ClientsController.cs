using BicycleCompany.BLL.Extensions;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
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
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientForReadModel))]
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
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientForReadModel))]
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
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        /// <response code="204">Client deleted successfully</response>
        /// <response code="400">Client model is invalid</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="204">Client deleted successfully</response>
        /// <response code="400">Client model is invalid</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    }
}
