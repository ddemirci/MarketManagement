using MarketManagement.Entities;
using MarketManagement.Models.Response;
using MarketManagement.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MarketManagement.Controllers
{
    [ApiController]
    [Route("users")]
    public class LoginController : ControllerBase
    {
        private readonly IService<User> _userService;
        public static IConfiguration _configuration { get; private set; }

        public LoginController(IService<User> service, IConfiguration configuration)
        {
            _userService = service;
            _configuration = configuration;
        }

        /// <summary>
        /// User Sign In
        /// </summary>
        /// <param name="signIn"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signin")]
        // POST: api/user/login
        public IActionResult SignIn([FromBody] User signIn)
        {
            var authResponse = AuthenticateUser(signIn).Result;

            if (authResponse.AuthResponseType == AuthResponseType.UserNotFound)
                return UserNotFound(signIn.PhoneNumber);
            else if (authResponse.AuthResponseType == AuthResponseType.UserCredentialsInvalid)
                return UserCredentialsWrong();
            else
            {
                var jwtString = GenerateJSONWebToken(signIn);
                return Ok(new { token = jwtString });
            }
        }

        /// <summary>
        /// Sign up a user
        /// </summary>
        /// <param name="signUp"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User signUp)
        {
            var result = await _userService.Create(signUp);
            if (result)
                return StatusCode(StatusCodes.Status201Created);
            else
                return BadRequest();
        }

        
        /// <summary>
        /// Returns all users defined
        /// </summary>
        /// <returns></returns>
        // GET: api/users
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();
            if (!users.Any())
            {
                return NotFound("There is no registered user");
            }
            return Ok(users);
        }


        private string GenerateJSONWebToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120), // Valid for 2 hours
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserAuthenticationResponse> AuthenticateUser(User login)
        {
            var response = new UserAuthenticationResponse(login.PhoneNumber);
            var user = await _userService.Get(login.PhoneNumber);

            if (user == null)
                response.AuthResponseType = AuthResponseType.UserNotFound;
            else
                response.AuthResponseType = user.Password.Equals(login.Password)
                    ? AuthResponseType.UserFound
                    : AuthResponseType.UserCredentialsInvalid;
            return response;
        }

        private IActionResult UserCredentialsWrong()
        {
            return NotFound("You have entered wrong user credentials");
        }

        private IActionResult UserNotFound(string phoneNumber)
        {
            return NotFound(string.Format("There is no user with phone number {0}", phoneNumber));
        }
    }



}
