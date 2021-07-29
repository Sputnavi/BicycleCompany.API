using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(IUserService userService, IAuthenticationManager authenticationManager)
        {
            _userService = userService;
            _authenticationManager = authenticationManager;
        }

        /// <summary>
        /// Register new User.
        /// </summary>
        /// <param name="userForRegistration">The user data for registration</param>
        /// <response code="201">User registered successfully</response> 
        /// <response code="400">User data is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationModel userForRegistration)
        {
            await _userService.CreateUserAsync(userForRegistration);

            return StatusCode(201);
        }

        /// <summary>
        /// Authenticate User.
        /// </summary>
        /// <param name="user">The user data for authentication</param>
        /// <response code="200">User authorized successfully</response> 
        /// <response code="401">User data is invalid</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationModel user)
        {
            if (!await _authenticationManager.ValidateUser(user))
            {
                return Unauthorized();
            }

            return Ok(new { Token = _authenticationManager.CreateToken() });
        }
    }
}
