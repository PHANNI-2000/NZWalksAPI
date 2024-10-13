using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace NZWalksAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
