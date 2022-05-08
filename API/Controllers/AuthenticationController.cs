using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using System.Text.Json;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin; 
using Microsoft.AspNet.Identity.EntityFramework;
using Core.Auth;
using Core.Interfaces;
using Core.Specifications.UserSpecifications;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Core.Interfaces;

namespace API.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationHelpers _authHelpers;
        private readonly IGenericRepository<User> _repo;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            IGenericRepository<User> repo, 
            IConfiguration configuration, 
            ILogger<AuthenticationController> logger,
            IAuthenticationHelpers authHelpers
        ) {
            _configuration = configuration;
            _repo = repo;
            _logger = logger;
            _authHelpers = authHelpers;
        }

        [HttpPost("signin")]
        public async Task<ActionResult> SignIn([FromBody] Credential credential) {
            string hashedPassword = _authHelpers.GenerateHash(credential.Password);

            var spec = new UsersFromCredentialSpecification(new Credential(
                Email: credential.Email,
                Password: hashedPassword
            ));
            var user = await _repo.GetEntityWithSpec(spec);

            if (user == null) {
                _logger.LogInformation("user with password does not exist");
                return Unauthorized();
            }

            _logger.LogInformation("user with password exists");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            

            var expiresAtTime = DateTime.UtcNow.AddMinutes(60);
             
            return Ok(new {
                accessToken = _authHelpers.CreateToken(claims, expiresAtTime),
                expiresAt = expiresAtTime
            });

        }
    }
}