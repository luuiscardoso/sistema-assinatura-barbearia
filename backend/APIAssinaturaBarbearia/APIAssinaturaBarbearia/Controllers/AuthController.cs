using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;


namespace APIAssinaturaBarbearia.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public AuthController(IUserService userService, IAdminService adminService)
        {
            _userService = userService;
            _adminService = adminService;
        }

        /// <summary>
        /// Realiza o login de um usuário e retorna o access token e refresh token.
        /// </summary>
        /// <param name="loginDTO">Email e senha do usuário</param>
        /// <returns>Access token e refresh token.</returns>
        #region AllUsers
        [HttpPost("sessions")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            TokenResponseDTO tokenResponseDTO = await _userService.LoginAsync(loginDTO);

            return Ok(tokenResponseDTO);
        }

        /// <summary>
        /// Renova o access token com o refresh token do usuário.
        /// </summary>
        /// <param name="tokenDTO">Access token e refresh token.</param>
        /// <returns>Novos access token e refresh token.</returns>
        [HttpPost("sessions/refresh")]
        public async Task<ActionResult> RenovarToken(TokenRequestDTO tokenDTO)
        {
            TokenResponseDTO tokenResponseDTO = await _userService.RenovarTokenAsync(tokenDTO);

            return Ok(tokenResponseDTO); 
        }

        /// <summary>
        /// Muda a senha do usuário admin.
        /// </summary>
        /// <param name="model">Email, senha atual e nova senha do usuário admin.</param>
        [HttpPost("admins/password-resets")]
        public async Task<ActionResult> RedefinirSenha(BarbeiroRedefinicaoSenhaDTO model)
        {
            IEnumerable<string> result = await _userService.RedefinirSenhaAsync(model);

            if (result.Any())
            {
                var erros = result;
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        /// <summary>
        /// Faz o logout de um usuário.
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <remarks>A desconexão de um usuário do sistema pode ser feita 
        /// por um terceiro desde que o mesmo tenha papel de administrador.</remarks>
        [Authorize]
        [HttpDelete("sessions")]
        public async Task<ActionResult> RevogarRefreshToken(string email)
        {
            await _userService.RevogarRefreshTokenAsync(email, User);

            return NoContent();
        }

        /// <summary>
        /// Gera token reset de senha e envia para o email do usuário.
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <returns></returns>
        [HttpPost("password-resets")]
        public async Task<ActionResult<string>> GerarTokenResetSenha(string email)
        {
            await _userService.GerarTokenResetSenhaAsync(email);

            return Ok();
        }

        /// <summary>
        /// Valida token antes de redirecionar para a página de reset de senha.
        /// </summary>
        /// <param name="token">Token recebido no email para reset de senha.</param>
        /// <returns></returns>
        [HttpPost("password-resets/token-valid")]
        public async Task<ActionResult<string>> VerificaTokenResetSenha(string token)
        {
            await _userService.VerificaTokenResetSenhaAsync(token);

            return Ok();
        }

        /// <summary>
        /// Reseta a senha do usuário.
        /// </summary>
        /// <param name="resetSenhaDTO">Token, e-mail e senha.</param>
        /// <returns></returns>
        [HttpPost("users/password-resets")]
        public async Task<ActionResult<string>> ResetarSenha(ResetSenhaDTO resetSenhaDTO)
        {
            await _userService.ResetSenhaAsync(resetSenhaDTO);

            return Ok();
        }
        #endregion

        #region AdminsOnly

        /// <summary>
        /// Registra um usuário.
        /// </summary>
        /// <param name="usuarioCadastroDTO">Email, senha, cpf e nome.</param>
        /// <returns></returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("admins/user")]
        public async Task<ActionResult> AdminCriarUsuario(UsuarioCadastroDTO usuarioCadastroDTO)
        {
            IEnumerable<string> result = await _adminService.CriarUsuarioAsync(usuarioCadastroDTO);

            if (result.Any())
            {
                var erros = result;
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        /// <summary>
        /// Cria um novo perfil no sistema.
        /// </summary>
        /// <param name="perfil">Novo perfil.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("admins/profile")]
        public async Task<ActionResult> CriarPerfil(string perfil) 
        {
            IEnumerable<string> result = await _adminService.CriarPerfilAsync(perfil);

            if (result.Any())
            {
                var erros = result;
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um perfil existente do sistema.
        /// </summary>
        /// <param name="perfil">Perfil a ser excluido.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("admins/profile")]
        public async Task<ActionResult> DeletarPerfil(string perfil)
        {
            var result = await _adminService.DeletarPerfilAsync(perfil);

            if (result.Any())
            {
                var erros = result;
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        /// <summary>
        /// Atribui a um usuário um perfil existente.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <param name="role">Perfil a ser associado.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("admins/user-role")]
        public async Task<ActionResult> AssociarPerfilUsuario(string email, string role)
        {
            var result = await _adminService.AssociarPerfilUsuarioAsync(email, role);

            if (result.Any())
            {
                var erros = result;
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        /// <summary>
        /// Deleta um usuário do sistema.
        /// </summary>
        /// <param name="email">Email do usuário a ser deletado.</param>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("admins/user")]
        public async Task<ActionResult> DeletarUsuario(string email)
        {
            IEnumerable<string> result = await _adminService.DeletarUsuarioAsync(email);

            if (result.Any())
            {
                var erros = result;
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }
        #endregion
    }
}
