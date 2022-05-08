using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Core.Interfaces
{
    public interface IAuthenticationHelpers
    {
        string CreateToken(IEnumerable<Claim> claims, DateTime expiresIn);
        string GenerateHash(String password);

        Boolean DoesUserHaveAccess(ClaimsIdentity identity, int Id);
    }
}