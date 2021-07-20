using AutoMapper;
using BicycleCompany.BLL.ActionFilters;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BicyclesController : ControllerBase
    {
        private readonly IBicycleService _bicycleService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public BicyclesController(ILoggerManager logger, IMapper mapper, IBicycleService bicycleService)
        {
            _logger = logger;
            _mapper = mapper;
            _bicycleService = bicycleService;
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
            var bicycles = await _bicycleService.GetBicyclesListAsync();

            return Ok(bicycles);
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
            var bicycleEntity = await _bicycleService.GetBicycleAsync(id);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                return NotFound("Bicycle with provided id cannot be found!");
            }

            return Ok(bicycleEntity);
        }

        /// <summary>
        /// Create new Bicycle.
        /// </summary>
        /// <param name="bicycle">The Bicycle object for creation</param>
        /// <response code="201">Bicycle created successfully</response> 
        /// <response code="400">Bicycle model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateBicycle([FromBody] BicycleForCreateOrUpdateModel bicycle)
        {
            var bicycleToReturn = await _bicycleService.CreateBicycleAsync(bicycle);

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
            var bicycleEntity = await _bicycleService.DeleteBicycleAsync(id);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                return NotFound("Bicycle with provided id cannot be found!");
            }

            return NoContent();
        }

        /// <summary>
        /// Update Bicycle information.
        /// </summary>
        /// <param name="id">The value that is used to find Bicycle</param>
        /// <param name="bicycle">The Bicycle object which is used for update Bicycle with provided id</param>
        /// <response code="204">Bicycle deleted successfully</response>
        /// <response code="400">Bicycle model is invalid</response>
        /// <response code="404">Bicycle with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateBicycle(Guid id, [FromBody] BicycleForCreateOrUpdateModel bicycle)
        {
            var bicycleEntity = await _bicycleService.UpdateBicycleAsync(id, bicycle);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Bicycle with id: {id} doesn't exist in the database.");
                return NotFound("Bicycle with provided id cannot be found!");
            }

            return NoContent();
        }
        
        /// <summary>
        /// Partially update Bicycle information.
        /// </summary>
        /// <param name="id">The value that is used to find Bicycle</param>
        /// <param name="patchDoc">The document with an array of operations for Bicycle with provided id</param>
        /// <response code="204">Bicycle deleted successfully</response>
        /// <response code="400">Bicycle model is invalid</response>
        /// <response code="404">Bicycle with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateBicycle(Guid id,
            [FromBody] JsonPatchDocument<BicycleForCreateOrUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var bicycleEntity = await _bicycleService.GetBicycleAsync(id);
            if (bicycleEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound("Client with provided id cannot be found!");
            }
            var bicycleToPatch = _mapper.Map<BicycleForCreateOrUpdateModel>(bicycleEntity);

            patchDoc.ApplyTo(bicycleToPatch, ModelState);

            TryValidateModel(bicycleToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bicycleService.UpdateBicycleAsync(id, bicycleToPatch);

            return NoContent();
        }
    }
}
