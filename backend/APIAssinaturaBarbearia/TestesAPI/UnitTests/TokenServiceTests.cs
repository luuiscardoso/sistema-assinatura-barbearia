using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using APIAssinaturaBarbearia.Application.Services;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace TestesAPI.UnitTests
{
    public class TokenServiceTests
    {
        public JwtSecurityToken CriaTokenAxuiliarValidacao(string chave, string alg)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            string? chaveSecretaTeste = chave;

            byte[] encodingChave = Encoding.UTF8.GetBytes(chaveSecretaTeste);

            SigningCredentials assinatura = new SigningCredentials(new SymmetricSecurityKey(encodingChave),
                                                                            alg);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = null,
                Expires = DateTime.UtcNow.AddSeconds(5),
                SigningCredentials = assinatura
            };

            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return token;
        }

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
                {"ChaveSecreta", "132002%12sau@p7LU123asoiaj~;.xci294" },
                {"ValidadeTokenMinutos", "2" }
            };

            var tokenService = new TokenService();

            //Act
            JwtSecurityToken result = tokenService.GerarToken(claims, tokenConfigs);
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

        [Fact]
        public void GeraToken_InformandoDadosConfigAusentes_RetornaException()
        {
            //Arrange
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "teste"),
                new Claim(ClaimTypes.Email, "teste@gmail.com")
            };

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"ValidadeTokenMinutos", "2"},
            };

            var tokenService = new TokenService();

            //Act & Assert
            Assert.ThrowsAny<Exception>(() => tokenService.GerarToken(claims, tokenConfigs));
        }

        [Fact]
        public void GeraToken_InformandoDadosFormatoInvalido_RetornaException()
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
                {"JWT:ValidadeTokenMinutos", "s"},
            };

            var tokenService = new TokenService();

            //Act & Assert
            Assert.ThrowsAny<Exception>(() => tokenService.GerarToken(claims, tokenConfigs));
        }

        [Fact]
        public void GeraToken_InformandoListaClaimsVazia_RetornaTokenSemClaimsPersonalizadas()
        {
            //Arrange 
            List<Claim> claims = new List<Claim>();

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"ChaveSecreta", "132002%12sau@p7LU123asoiaj~;.xci294"},
                {"ValidadeTokenMinutos", "2"},
            };

            var tokenService = new TokenService();

            //Act
            JwtSecurityToken result = tokenService.GerarToken(claims, tokenConfigs);
            IEnumerable<Claim> payloadClaims = result.Payload.Claims.ToList();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, payloadClaims.Count());
        }

        [Fact]
        public void ValidaToken_InformandoTokenValido_RetornaClaimsPrincipal()
        {
            //Arrange
            JwtSecurityToken token = CriaTokenAxuiliarValidacao("@120*4hHKm412@120*4hHKm412@120*4hHKm412", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256");
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"ChaveSecreta","@120*4hHKm412@120*4hHKm412@120*4hHKm412"}
            };

            var tokenService = new TokenService();

            //Act
            ClaimsPrincipal result = tokenService.ValidaTokenObtemClaims(tokenString, tokenConfigs);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]  
        public void ValidaToken_InformandoTokenComAssinaturaInvalidaPorChaveSecretaDiferente_RetornaException()
        {
            //Arrange
            JwtSecurityToken token = CriaTokenAxuiliarValidacao("@120*4hHKm412@120*4hHKm412@120*4hHKm412", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256");
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"ChaveSecreta","*&ghfjhg6454%a2%!2@1@@ghfjhg64543m410"}
            };

            var tokenService = new TokenService();

            //Act & Assert
            Assert.ThrowsAny<Exception>(() => tokenService.ValidaTokenObtemClaims(tokenString, tokenConfigs));
        }

        [Fact]
        public void ValidaToken_InformandoTokenComAssinaturaInvalidaPorConteudoAlterado_RetornaException()
        {
            //Arrange
            JwtSecurityToken token = CriaTokenAxuiliarValidacao("@120*4hHKm412@120*4hHKm412@120*4hHKm412", "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256");
            token.Payload.AddClaim(new Claim("novoValor", "teste"));
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"ChaveSecreta","*&ghfjhg6454%a2%!2@1@@ghfjhg64543m410"}
            };

            var tokenService = new TokenService();

            //Act & Assert
            Assert.ThrowsAny<Exception>(() => tokenService.ValidaTokenObtemClaims(tokenString, tokenConfigs));
        }

        [Fact]
        public void ValidaToken_InformandoTokenComAlgoritmoDiferente_RetornaException()
        {
            //Arrange
            JwtSecurityToken token = CriaTokenAxuiliarValidacao("@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412", 
                                                                 "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384");

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Dictionary<string, string> tokenConfigs = new Dictionary<string, string>()
            {
                {"ChaveSecreta","@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412@120*4hHKm412"}
            };

            var tokenService = new TokenService();

            //Act & Assert
            Assert.ThrowsAny<Exception>(() => tokenService.ValidaTokenObtemClaims(tokenString, tokenConfigs));
        }

        
    } 
}
