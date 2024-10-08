using APIAssinaturaBarbearia.Services.Interfaces;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIAssinaturaBarbearia.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GerarToken(List<Claim> claims, IConfiguration config)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string? key = config["JWT:ChaveSecreta"];

            byte[] keyEncoding = Encoding.UTF8.GetBytes(key!);

            SigningCredentials assinatura = new SigningCredentials(new SymmetricSecurityKey(keyEncoding), SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(config.GetSection("JWT").GetValue<double>("ValidadeTokenMinutos")),
                SigningCredentials = assinatura
            };

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return token;
        }
    }
}
