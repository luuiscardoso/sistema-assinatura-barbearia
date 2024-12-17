using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using APIAssinaturaBarbearia.Application.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace APIAssinaturaBarbearia.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserService(ITokenService tokenService, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<TokenResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (usuario == null) throw new ApplicationUserNotRegisteredException("Usuário não cadastrado, contate o administrador.");

            if (!await _userManager.CheckPasswordAsync(usuario, loginDTO.Senha) || usuario.Email != loginDTO.Email)
                throw new ApplicationInvalidCredentialsException("Credenciais inválidas.");

            var roles = await _userManager.GetRolesAsync(usuario);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            Dictionary<string, string> configuracoes = new Dictionary<string, string>()
            {
                {"ChaveSecreta", _configuration["JWT:ChaveSecreta"] },
                {"ValidadeTokenMinutos", _configuration["JWT:ValidadeTokenMinutos"] },
                {"ValidadeRefreshTokenMinutos", _configuration["JWT:ValidadeRefreshTokenMinutos"] }
            };

            JwtSecurityToken token = _tokenService.GerarToken(claims, configuracoes);

            string refreshToken = _tokenService.GerarRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenTempoExpiracao = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:ValidadeTokenMinutos"]));

            await _userManager.UpdateAsync(usuario);

            return new TokenResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiracao = token.ValidTo
            };
        }

        public async Task<TokenResponseDTO> RenovarTokenAsync(TokenRequestDTO tokenDTO)
        {
            Dictionary<string, string> configuracoes = new Dictionary<string, string>()
            {
                {"ChaveSecreta", _configuration["ChaveSecreta"] },
                {"ValidadeTokenMinutos", _configuration["ValidadeTokenMinutos"] },
                {"ValidadeRefreshTokenMinutos", _configuration["ValidadeRefreshTokenMinutos"] }
            };

            ClaimsPrincipal principal = _tokenService.ValidaTokenObtemClaims(tokenDTO.TokenPrincipal, configuracoes);

            string email = principal.FindFirst(ClaimTypes.Email)!.Value;

            Usuario? usuario = await _userManager.FindByEmailAsync(email);

            if (usuario.RefreshToken == null || !usuario.RefreshToken.Equals(tokenDTO.RefreshToken) || DateTime.UtcNow >= usuario.RefreshTokenTempoExpiracao)
            {
                throw new ApplicationInvalidRefreshTokenException("Refresh Token inválido");
            }

            JwtSecurityToken token = _tokenService.GerarToken(principal.Claims.ToList(), configuracoes);

            string refreshToken = _tokenService.GerarRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenTempoExpiracao = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWTValidadeRefreshTokenMinutos"]));

            await _userManager.UpdateAsync(usuario);

            return new TokenResponseDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiracao = token.ValidTo
            };
        }

        public async Task<IEnumerable<string>> RedefinirSenhaAsync(BarbeiroRedefinicaoSenhaDTO model)
        {
            Usuario? barbeiro = await _userManager.FindByEmailAsync(model.Email);

            if (barbeiro is null)
            {
                throw new ApplicationNotFoundException("Usuário inexistente.");
            }

            var result = await _userManager.ChangePasswordAsync(barbeiro, model.SenhaAtual, model.NovaSenha);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return erros;
            }

            return Enumerable.Empty<string>();
        }

        public async Task RevogarRefreshTokenAsync(string email, ClaimsPrincipal httpContextUser)
        {
            Usuario? usuarioAutenticado = await _userManager.GetUserAsync(httpContextUser);

            if (!usuarioAutenticado.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                && !httpContextUser.IsInRole("Admin"))
            {
                throw new ApplicationRevocationAccessException("Não é possivel revogar o acesso, verifique o e-mail ou seu perfil de acesso.");
            }

            Usuario? usuarioRevogar = await _userManager.FindByEmailAsync(email);
            if (usuarioRevogar is null)
                throw new ApplicationNotFoundException("Usuário inexistente.");

            usuarioRevogar.RefreshToken = null;
            await _userManager.UpdateAsync(usuarioRevogar);
        }
    }
}
