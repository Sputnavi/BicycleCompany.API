using AutoMapper;
using BicycleCompany.BLL.ActionFilters;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IClientService _clientService;

        public ClientsController(IMapper mapper, ILoggerManager logger, IClientService clientService)
        {
            _mapper = mapper;
            _logger = logger;
            _clientService = clientService;
        }

        /// <summary>
        /// Return a list of all Clients.
        /// </summary>
        /// <response code="200">List of clients returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientService.GetClientsListAsync();

            return Ok(clients);
        }

        /// <summary>
        /// Return Client.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <response code="200">Client returned successfully</response> 
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}", Name = "GetClient")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var clientEntity = await _clientService.GetClientAsync(id);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound("Client with provided id cannot be found!");
            }

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateClient([FromBody] ClientForCreateOrUpdateModel client)
        {
            var clientToReturn = await _clientService.CreateClientAsync(client);

            return CreatedAtRoute("GetClient", new { id = clientToReturn.Id }, clientToReturn);
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
            var clientEntity = await _clientService.DeleteClientAsync(id);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound("Client with provided id cannot be found!");
            }

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientForCreateOrUpdateModel client)
        {
            var clientEntity = await _clientService.UpdateClientAsync(id, client);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound("Client with provided id cannot be found!");
            }

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
                return BadRequest("patchDoc object is null");
            }

            var clientEntity = await _clientService.GetClientAsync(id);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound("Client with provided id cannot be found!");
            }
            var clientToPatch = _mapper.Map<ClientForCreateOrUpdateModel>(clientEntity);

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
