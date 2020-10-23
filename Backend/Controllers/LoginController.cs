using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        private List<UserModel> _users = new List<UserModel>
        {
            new UserModel { Id = "1", Username = "adminas", Name = "Petras", Surname = "Jonaitis", Email = "petras.jonaitis@gmail.com", Password = "slaptas", UserRole = "Admin" },
            new UserModel { Id = "2", Username = "useris", Name = "Jonas", Surname = "Petraitis", Email = "jonas.petraitis@gmail.com", Password = "slaptesnis", UserRole = "User"  },
            new UserModel { Id = "3", Username = "svecias", Name = "Kazimieras", Surname = "Baryga", Email = "kazimieras.baryga@gmail.com", Password = "slapciausias", UserRole = "Guest"  }
        };

        public LoginController(IConfiguration config)
        { 
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            UserModel user = AuthenticateUser(login);

            if (user != null)
            {
                var generatedToken = GenerateJWTToken(user);

                response = Ok(new
                {
                    token = generatedToken,
                    userDetails = user,
                });
            }
            return response;
        }

        UserModel AuthenticateUser(UserModel loginCredentials)
        {
            UserModel user = _users.Where(x => x.Username == loginCredentials.Username && x.Password == loginCredentials.Password).SingleOrDefault();
            return user;
        }
        string GenerateJWTToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiryDuration = int.Parse(_config["Jwt:ExpiryDuration"]);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim("userid", userInfo.Id.ToString()),
                new Claim("role",userInfo.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.Now.AddMinutes(expiryDuration),
                IssuedAt = DateTime.Now,
                SigningCredentials = credentials
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = (JwtSecurityToken)jwtTokenHandler.CreateToken(tokenDescriptor);
            var tokenString = jwtTokenHandler.WriteToken(jwtToken);
            //var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            //var token = jwtTokenHandler.WriteToken(jwtToken);

            return tokenString;
        }
    }

}