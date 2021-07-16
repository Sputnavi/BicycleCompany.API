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
    public class BicyclesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public BicyclesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Return a list of all Bicycles.
        /// </summary>
        /// <response code="200">List of bicycles returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetBicycles()
        {
            var bicycles = await _repository.Bicycle.GetBicyclesAsync(trackChanges: false);
            var bicyclesModel = _mapper.Map<IEnumerable<BicycleForReadModel>>(bicycles);

            return Ok(bicyclesModel);
        }

        /// <summary>
        /// Return Bicycle.
        /// </summary>
        /// <param name="id">The value that is used to find bicycle</param>
        /// <response code="200">Bicycle returned successfully</response> 
        /// <response code="404">Bicycle with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}", Name = "GetBicycle")]
        public async Task<IActionResult> GetBicycle(Guid id)
        {
            var bicycleEntity = await _repository.Bicycle.GetBicycleAsync(id, trackChanges: false);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var bicycleModel = _mapper.Map<BicycleForReadModel>(bicycleEntity);
            return Ok(bicycleModel);
        }

        /// <summary>
        /// Create new Bicycle.
        /// </summary>
        /// <param name="bicycle">The Bicycle object for creation</param>
        /// <response code="201">Bicycle created successfully</response> 
        /// <response code="422">Bicycle model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateBicycle([FromBody] BicycleForCreationModel bicycle)
        {
            var bicycleEntiy = _mapper.Map<Bicycle>(bicycle);

            await _repository.Bicycle.CreateBicycleAsync(bicycleEntiy);

            var bicycleToReturn = _mapper.Map<BicycleForReadModel>(bicycleEntiy);

            return CreatedAtRoute("GetBicycle", new { id = bicycleToReturn.Id }, bicycleToReturn);
        }

        /// <summary>
        /// Delete Bicycle.
        /// </summary>
        /// <param name="id">The value that is used to find Bicycle</param>
        /// <response code="204">Bicycle deleted successfully</response>
        /// <response code="404">Bicycle with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBicycle(Guid id)
        {
            var bicycleEntity = await _repository.Bicycle.GetBicycleAsync(id, trackChanges: false);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            await _repository.Bicycle.DeleteBicycleAsync(bicycleEntity);

            return NoContent();
        }

        /// <summary>
        /// Update Bicycle information.
        /// </summary>
        /// <param name="id">The value that is used to find Bicycle</param>
        /// <param name="bicycle">The Bicycle object which is used for update Bicycle with provided id</param>
        /// <response code="204">Bicycle deleted successfully</response>
        /// <response code="404">Bicycle with provided id cannot be found!</response>
        /// <response code="422">Bicycle model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateBicycle(Guid id, [FromBody] BicycleForUpdateModel bicycle)
        {
            var bicycleEntity = await _repository.Bicycle.GetBicycleAsync(id, trackChanges: true);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            
            _mapper.Map(bicycle, bicycleEntity);
            await _repository.Bicycle.UpdateBicycleAsync(bicycleEntity);

            return NoContent();
        }

        /// <summary>
        /// Partially update Bicycle information.
        /// </summary>
        /// <param name="id">The value that is used to find Bicycle</param>
        /// <param name="patchDoc">The document with an array of operations for Bicycle with provided id</param>
        /// <response code="204">Bicycle deleted successfully</response>
        /// <response code="404">Bicycle with provided id cannot be found!</response>
        /// <response code="422">Bicycle model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateBicycle(Guid id,
            [FromBody] JsonPatchDocument<BicycleForUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var bicycleEntity = await _repository.Bicycle.GetBicycleAsync(id, trackChanges: true);
            var bicycleToPatch = _mapper.Map<BicycleForUpdateModel>(bicycleEntity);

            patchDoc.ApplyTo(bicycleToPatch, ModelState);

            TryValidateModel(bicycleToPatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(bicycleToPatch, bicycleEntity);

            await _repository.Bicycle.UpdateBicycleAsync(bicycleEntity);

            return NoContent();
        }
    }
}
