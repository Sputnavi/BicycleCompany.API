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
    public class PartsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IPartService _partService;

        public PartsController(IMapper mapper, ILoggerManager logger, IPartService partService)
        {
            _mapper = mapper;
            _logger = logger;
            _partService = partService;
        }

        /// <summary>
        /// Return a list of all Parts.
        /// </summary>
        /// <response code="200">List of parts returned successfully</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetParts()
        {
            var parts = await _partService.GetPartsListAsync();

            return Ok(parts);
        }

        /// <summary>
        /// Return Part.
        /// </summary>
        /// <param name="id">The value that is used to find part</param>
        /// <response code="200">Part returned successfully</response> 
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}", Name = "GetPart")]
        public async Task<IActionResult> GetPart(Guid id)
        {
            var partEntity = await _partService.GetPartAsync(id);
            if (partEntity is null)
            {
                _logger.LogInfo($"Part with id: {id} doesn't exist in the database.");
                return NotFound("Part with provided id cannot be found!");
            }

            return Ok(partEntity);
        }

        /// <summary>
        /// Create new Part.
        /// </summary>
        /// <param name="part">The Part object for creation</param>
        /// <response code="201">Part created successfully</response> 
        /// <response code="400">Part model is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePart([FromBody] PartForCreateOrUpdateModel part)
        {
            var partToReturn = await _partService.CreatePartAsync(part);

            return CreatedAtRoute("GetPart", new { id = partToReturn.Id }, partToReturn);
        }

        /// <summary>
        /// Delete Part.
        /// </summary>
        /// <param name="id">The value that is used to find Part</param>
        /// <response code="204">Part deleted successfully</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(Guid id)
        {
            var partEntity = await _partService.DeletePartAsync(id);
            if (partEntity is null)
            {
                _logger.LogInfo($"Part with id: {id} doesn't exist in the database.");
                return NotFound("Part with provided id cannot be found!");
            }

            return NoContent();
        }

        /// <summary>
        /// Update Part information.
        /// </summary>
        /// <param name="id">The value that is used to find Part</param>
        /// <param name="part">The Part object which is used for update Part with provided id</param>
        /// <response code="204">Part deleted successfully</response>
        /// <response code="400">Part model is invalid</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePart(Guid id, [FromBody] PartForCreateOrUpdateModel part)
        {
            var partEntity = await _partService.UpdatePartAsync(id, part);
            if (partEntity is null)
            {
                _logger.LogInfo($"Part with id: {id} doesn't exist in the database.");
                return NotFound("Part with provided id cannot be found!");
            }

            return NoContent();
        }

        /// <summary>
        /// Partially update Part information.
        /// </summary>
        /// <param name="id">The value that is used to find Part</param>
        /// <param name="patchDoc">The document with an array of operations for Part with provided id</param>
        /// <response code="204">Part deleted successfully</response>
        /// <response code="400">Part model is invalid</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdatePart(Guid id,
            [FromBody] JsonPatchDocument<PartForCreateOrUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var partEntity = await _partService.GetPartAsync(id);
            if (partEntity is null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound("Client with provided id cannot be found!");
            }
            var partToPatch = _mapper.Map<PartForCreateOrUpdateModel>(partEntity);

            patchDoc.ApplyTo(partToPatch, ModelState);

            TryValidateModel(partToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _partService.UpdatePartAsync(id, partToPatch);

            return NoContent();
        }
    }
}
