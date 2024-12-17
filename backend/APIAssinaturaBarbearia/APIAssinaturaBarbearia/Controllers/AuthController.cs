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
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public AuthController(IUserService userService, IAdminService adminService)
        {
            _userService = userService;
            _adminService = adminService;
        }

        #region AllUsers
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            TokenResponseDTO tokenResponseDTO = await _userService.LoginAsync(loginDTO);

            return Ok(tokenResponseDTO);
        }

        [HttpPost("RenovarToken")]
        public async Task<ActionResult> RenovarToken(TokenRequestDTO tokenDTO)
        {
            TokenResponseDTO tokenRequestDTO = await _userService.RenovarTokenAsync(tokenDTO);

            return Ok(tokenRequestDTO); 
        }

        [HttpPost("RedefinirSenha")]
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

        [Authorize]
        [HttpPost("RevogarAcesso")]
        public async Task<ActionResult> RevogarRefreshToken(string email)
        {
            await _userService.RevogarRefreshTokenAsync(email, User);

            return NoContent();
        }
        #endregion

        #region AdminsOnly

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("Admin/RegistroUsuario")]
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

        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("Admin/CriarPerfil")]
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

        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("Admin/ExcluirPerfil")]
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


        [Authorize(Policy = "AdminOnly")]
        [HttpPost("Admin/AssociarPerfilUsuario")]
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

        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("Admin/ExcluirUsuario")]
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
