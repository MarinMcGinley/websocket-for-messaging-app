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

namespace API.Controllers
{
    public class AuthenticationController : BaseApiController
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IGenericRepository<User> _repo;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IGenericRepository<User> repo, IConfiguration configuration, ILogger<AuthenticationController> logger) {
            _configuration = configuration;
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("signin")]
        public async Task<ActionResult> SignIn(Credential userInfo) {
            // _logger.LogInformation("signing in");
            // _logger.LogInformation(JsonSerializer.Serialize(userInfo));
            // // var user = new IdentityUser { Email = userInfo.Email };
            // // var result = await UserManager.CreateAsync(user, userInfo.Password);
            // // var signinResults = await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: true);
            // // _logger.LogInformation(JsonSerializer.Serialize(signinResults));

            // var userStore = new UserStore<IdentityUser>();
            // var userManager = new UserManager<IdentityUser>(userStore);
            // var user = userManager.Find(userInfo.Email, userInfo.Password);
            // if (user != null) {
            //     var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //     var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

            //     authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
            //     _logger.LogInformation("Able to sign user in!!");
            // }
            return Ok();
        }

        [HttpPost("second-signin")]
        public async Task<ActionResult> SecondSignIn([FromBody] Credential credential) {
            var spec = new UsersFromCredentialSpecification(credential);
            var user = await _repo.GetEntityWithSpec(spec);

            if (user == null) {
                _logger.LogInformation("user with password does not exist");
                return Unauthorized();
            }

            _logger.LogInformation("user with password exists");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var expiresAtTime = DateTime.UtcNow.AddMinutes(5);
             
            return Ok(new {
                accessToken = CreateToken(claims, expiresAtTime),
                expiresAt = expiresAtTime
            });

        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresIn) {
            var secretKey = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresIn,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}