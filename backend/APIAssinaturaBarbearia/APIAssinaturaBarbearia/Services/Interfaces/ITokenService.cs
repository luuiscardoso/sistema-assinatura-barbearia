using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GerarToken(List<Claim> claims, IConfiguration config);

        string GerarRefreshToken();

        ClaimsPrincipal ValidaTokenObtemClaims(string token, IConfiguration config);
    }
}
