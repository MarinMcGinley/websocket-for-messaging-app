using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class AuthenticationHelpers : IAuthenticationHelpers
    {
        private readonly IConfiguration _configuration;
        public AuthenticationHelpers(IConfiguration configuration) {
            _configuration = configuration;

        }

        public Boolean DoesUserHaveAccess(ClaimsIdentity identity, int Id) {
            // var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return false;

            if (Id.ToString() == identity.FindFirst("UserId").Value) return true;

            return false;
        }

        public string CreateToken(IEnumerable<Claim> claims, DateTime expiresIn) {
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

        public string GenerateHash(String password) {
            var salt = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Salt"));

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8)
            );

            return hashed;
        }
    }
}