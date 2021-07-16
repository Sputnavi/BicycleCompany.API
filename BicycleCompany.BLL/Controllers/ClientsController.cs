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
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public ClientsController(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Return a list of all Clients.
        /// </summary>
        /// <response code="200">List of clients returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _repository.Client.GetClientsAsync(trackChanges: false);
            var clientsModel = _mapper.Map<IEnumerable<ClientForReadModel>>(clients);

            return Ok(clientsModel);
        }

        /// <summary>
        /// Return Client.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <response code="200">Client returned successfully</response> 
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id}", Name = "GetClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var clientEntity = await _repository.Client.GetClientAsync(id, trackChanges: false);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var clientModel = _mapper.Map<ClientForReadModel>(clientEntity);
            return Ok(clientModel);
        }

        /// <summary>
        /// Create new Client.
        /// </summary>
        /// <param name="client">The client object for creation</param>
        /// <response code="201">Client created successfully</response> 
        /// <response code="422">Client model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateClient([FromBody] ClientForCreationModel client)
        {
            var clientEntity = _mapper.Map<Client>(client);

            await _repository.Client.CreateClientAsync(clientEntity);

            var clientToReturn = _mapper.Map<ClientForReadModel>(clientEntity);

            return CreatedAtRoute("GetClient", new { id = clientToReturn.Id }, clientToReturn);
        }

        /// <summary>
        /// Delete Client.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <response code="204">Client deleted successfully</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var clientEntity = await _repository.Client.GetClientAsync(id, trackChanges: false);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            await _repository.Client.DeleteClientAsync(clientEntity);

            return NoContent();
        }

        /// <summary>
        /// Update Client information.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <param name="client">The client object which is used for update client with provided id</param>
        /// <response code="204">Client deleted successfully</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="422">Client model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientForUpdateModel client)
        {
            var clientEntity = await _repository.Client.GetClientAsync(id, trackChanges: true);
            if (clientEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(client, clientEntity);
            await _repository.Client.UpdateClientAsync(clientEntity);

            return NoContent();
        }

        /// <summary>
        /// Partially update Client information.
        /// </summary>
        /// <param name="id">The value that is used to find client</param>
        /// <param name="patchDoc">The document with an array of operations for client with provided id</param>
        /// <response code="204">Client deleted successfully</response>
        /// <response code="404">Client with provided id cannot be found!</response>
        /// <response code="422">Client model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PartiallyUpdateClient(Guid id, 
            [FromBody] JsonPatchDocument<ClientForUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var clientEntity = await _repository.Client.GetClientAsync(id, trackChanges: true);
            var clientToPatch = _mapper.Map<ClientForUpdateModel>(clientEntity);

            patchDoc.ApplyTo(clientToPatch, ModelState);

            TryValidateModel(clientToPatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(clientToPatch, clientEntity);

            await _repository.Client.UpdateClientAsync(clientEntity);

            return NoContent();
        }
    }
}
