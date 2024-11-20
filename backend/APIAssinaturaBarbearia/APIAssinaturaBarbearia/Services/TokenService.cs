using APIAssinaturaBarbearia.Services.Interfaces;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public string GerarRefreshToken()
        {
            byte[] arrayNumerosAleatorios = new byte[128];

            using var numerosAleatorios = RandomNumberGenerator.Create();

            numerosAleatorios.GetBytes(arrayNumerosAleatorios);

            string refreshToken = Convert.ToBase64String(arrayNumerosAleatorios);
            
            return refreshToken;
        }

        public ClaimsPrincipal ValidaTokenObtemClaims(string token, IConfiguration config)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();   
            
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:ChaveSecreta"]!))
            };

            //retorna excessão caso o token não seja válido
            ClaimsPrincipal principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            JwtSecurityToken? jwtToken = validatedToken as JwtSecurityToken;

            if(jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido");
            }

            return principal;
        }
    }
}
