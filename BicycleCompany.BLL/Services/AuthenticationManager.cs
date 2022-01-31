using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordManager _passwordManager;

        private User _user;

        public AuthenticationManager(IUserRepository userRepository, IConfiguration configuration, IPasswordManager passwordManager)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordManager = passwordManager;
        }

        /// <summary>
        /// Create token for authentication.
        /// </summary>
        public string CreateToken()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: GetClaims(),
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: GetSigningCredentials()
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        /// <summary>
        /// Check if user exists.
        /// </summary>
        /// <param name="userForAuthentication">Model with data for authentication.</param>
        /// <returns>True if the user exists; otherwise False</returns>
        public async Task<bool> ValidateUser(UserForAuthenticationModel userForAuthentication)
        {
            _user = await _userRepository.GetUserByLoginAsync(userForAuthentication.Login);

            var providedPasswordHash = _passwordManager.GetPasswordHash(userForAuthentication.Password, _user.Salt);

            return (_user != null && _user.Password == providedPasswordHash);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings").GetSection("key").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, _user.Id.ToString())
            };

            if (_user.Role != null)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, _user.Role));
            }

            return claims;
        }
    }
}
