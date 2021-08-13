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
    [Authorize(Roles = "Administrator, Master")]
    [SwaggerTag("Master")]
    [Route("api/parts")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IPartService _partService;

        public PartsController(ILoggerManager logger, IPartService partService)
        {
            _logger = logger;
            _partService = partService;
        }

        /// <summary>
        /// Return a list of all Parts.
        /// </summary>
        /// <response code="200">List of parts returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PartForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [AllowAnonymous]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetPartList([FromQuery]PartParameters partParameters)
        {
            var parts = await _partService.GetPartListAsync(partParameters, Response);

            return Ok(parts);
        }

        /// <summary>
        /// Return Part.
        /// </summary>
        /// <param name="id">The value that is used to find part</param>
        /// <response code="200">Part returned successfully</response> 
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PartForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetPart")]
        public async Task<IActionResult> GetPart(Guid id)
        {
            var partEntity = await _partService.GetPartAsync(id);

            return Ok(partEntity);
        }

        /// <summary>
        /// Create new Part.
        /// </summary>
        /// <param name="part">The Part object for creation</param>
        /// <response code="201">Part created successfully</response> 
        /// <response code="400">Part model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddedResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPost]
        public async Task<IActionResult> CreatePart([FromBody] PartForCreateOrUpdateModel part)
        {
            this.ValidateObject();

            var partId = await _partService.CreatePartAsync(part);

            return CreatedAtRoute("GetPart", new { id = partId }, new AddedResponse(partId));
        }

        /// <summary>
        /// Delete Part.
        /// </summary>
        /// <param name="id">The value that is used to find Part</param>
        /// <response code="204">Part deleted successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart(Guid id)
        {
            await _partService.DeletePartAsync(id);
            
            return NoContent();
        }

        /// <summary>
        /// Update Part information.
        /// </summary>
        /// <param name="id">The value that is used to find Part</param>
        /// <param name="part">The Part object which is used for update Part with provided id</param>
        /// <response code="204">Part updated successfully</response>
        /// <response code="400">Part model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePart(Guid id, [FromBody] PartForCreateOrUpdateModel part)
        {
            this.ValidateObject();

            await _partService.UpdatePartAsync(id, part);

            return NoContent();
        }

        /// <summary>
        /// Partially update Part information.
        /// </summary>
        /// <param name="id">The value that is used to find Part</param>
        /// <param name="patchDoc">The document with an array of operations for Part with provided id</param>
        /// <response code="204">Part updated successfully</response>
        /// <response code="400">Part model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">Part with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponseModel))]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdatePart(Guid id,
            [FromBody] JsonPatchDocument<PartForCreateOrUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("Sent patch document is empty.");
            }

            var partToPatch = await _partService.GetPartForUpdateModelAsync(id);

            patchDoc.ApplyTo(partToPatch, ModelState);

            TryValidateModel(partToPatch);
            this.ValidateObject();

            await _partService.UpdatePartAsync(id, partToPatch);

            return NoContent();
        }
    }
}
