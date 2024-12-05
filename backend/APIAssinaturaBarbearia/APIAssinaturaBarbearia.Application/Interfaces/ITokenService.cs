using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GerarToken(List<Claim> claims, Dictionary<string, string> configuracoes);

        string GerarRefreshToken();

        ClaimsPrincipal ValidaTokenObtemClaims(string token, Dictionary<string, string> configuracoes);
    }
}
