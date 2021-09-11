using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Owleye.Shared.Base;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Owleye.API.Owleye.API.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : BaseController
    {
        private IConfiguration _config;

        public AuthController(IConfiguration config, IAppSession appSession, IHttpContextAccessor httpContextAccessor) : base(appSession, httpContextAccessor)
        {
            _config = config;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserDto login)
        {

            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateToken(user);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }

        private string GenerateToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Email", user.EmailAddress),
                new Claim("Id", user.Id.ToString()),
             };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddHours(5),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDto AuthenticateUser(UserDto user)
        {

            if (user.Username == "owleye" && user.Password == "123")
            {
                user = new UserDto { Username = "owl eye user", EmailAddress = "xx@yy.com", Id = 1 };
                return user;
            }

            return null;
        }
    }
}
