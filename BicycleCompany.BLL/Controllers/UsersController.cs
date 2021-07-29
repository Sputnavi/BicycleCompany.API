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
    [Authorize(Roles = "Administrator")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IUserService _userService;

        public UsersController(ILoggerManager logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Return a list of all Users.
        /// </summary>
        /// <response code="200">List of users returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetUserList([FromQuery] UserParameters userParameters)
        {
            var users = await _userService.GetUserListAsync(userParameters, Response);

            return Ok(users);
        }

        /// <summary>
        /// Return User.
        /// </summary>
        /// <param name="id">The value that is used to find user</param>
        /// <response code="200">User returned successfully</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">User with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserForReadModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userEntity = await _userService.GetUserAsync(id);

            return Ok(userEntity);
        }

        /// <summary>
        /// Update User information.
        /// </summary>
        /// <param name="id">The value that is used to find User</param>
        /// <param name="user">The User object which is used for update User with provided id</param>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">User model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">User with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserForUpdateModel user)
        {
            this.ValidateObject();

            await _userService.UpdateUserAsync(id, user);

            return NoContent();
        }

        /// <summary>
        /// Partially update User information.
        /// </summary>
        /// <param name="id">The value that is used to find User</param>
        /// <param name="patchDoc">The document with an array of operations for User with provided id</param>
        /// <response code="204">User updated successfully</response>
        /// <response code="400">User model is invalid</response>
        /// <response code="401">You need to authorize first</response>
        /// <response code="403">Your role dosn't have enough rights</response>
        /// <response code="404">User with provided id cannot be found!</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateUser(Guid id,
            [FromBody] JsonPatchDocument<UserForUpdateModel> patchDoc)
        {
            if (patchDoc is null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("Sent patch document is empty.");
            }

            var userToPatch = await _userService.GetUserForUpdateModelAsync(id);

            patchDoc.ApplyTo(userToPatch, ModelState);

            TryValidateModel(userToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(id, userToPatch);

            return NoContent();
        }
    }
}
