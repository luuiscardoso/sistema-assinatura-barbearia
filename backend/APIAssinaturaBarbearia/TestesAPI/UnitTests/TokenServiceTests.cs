using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using APIAssinaturaBarbearia.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace TestesAPI.UnitTests
{
    public class TokenServiceTests
    {
        [Fact]
        public void GeraToken_InformandoListaClaimsValida_RetornaJwtSecutiryToken()
        {
            //Arrange 
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "teste"),
                new Claim(ClaimTypes.Email, "teste@gmail.com")
            };

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"JWT:ChaveSecreta", "132002%12sau@p7LU123asoiaj~;.xci294"},
                {"JWT:ValidadeTokenMinutos", "2"},
                {"JWT:ValidadeRefreshTokenMinutos", "30"}
            };

            IConfiguration tokenConfig = new ConfigurationManager().AddInMemoryCollection(tokenConfigs).Build();

            var tokenService = new TokenService();

            //Act
            JwtSecurityToken result = tokenService.GerarToken(claims, tokenConfig);
            IEnumerable<string> payloadClaimsTypes = result.Payload.Claims.ToList().Select(c => c.Type);
            TimeSpan tempoExpiracaoToken = result.ValidTo - result.ValidFrom;
            TimeSpan entreInicio = new TimeSpan(0, 1, 55);
            TimeSpan entreFim = new TimeSpan(0, 2, 05);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(SecurityAlgorithms.HmacSha256, result.Header.Alg); 
            Assert.InRange<TimeSpan>(tempoExpiracaoToken, entreInicio, entreFim); //expiracao esta no intervalo correto
            Assert.Contains<string>("unique_name", payloadClaimsTypes);
            Assert.Contains<string>("email", payloadClaimsTypes);
        }
    } 
}
