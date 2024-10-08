﻿using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIAssinaturaBarbearia.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        #region AllUsers
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (usuario == null) return BadRequest("Usuário não cadastrado, contate o administrador.");

            if (!await _userManager.CheckPasswordAsync(usuario, loginDTO.Senha) || usuario.Email != loginDTO.Email)
                return Unauthorized("Credenciais inválidas.");

            var roles = await _userManager.GetRolesAsync(usuario);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken token = _tokenService.GerarToken(claims, _configuration);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Exp = token.ValidTo
            });
        }

        [HttpPost("RedefinirSenha")]
        public async Task<ActionResult> RedefinirSenha(BarbeiroRedefinicaoSenhaDTO model)
        {
            Usuario? barbeiro = await _userManager.FindByEmailAsync(model.Email);

            if (barbeiro is null) return BadRequest("Usuário inexistente.");

            var result = await _userManager.ChangePasswordAsync(barbeiro, model.SenhaAtual, model.NovaSenha);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }
        #endregion

        #region AdminsOnly

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("Admin/RegistroUsuario")]
        public async Task<ActionResult> AdminCriarUsuario(UsuarioCadastroDTO model)
        {
            Usuario? barbeiroExiste = await _userManager.FindByEmailAsync(model.Email);

            if (barbeiroExiste != null) return BadRequest("Usuário já cadastrado");

            Usuario barbeiro = new Usuario()
            {
                Email = model.Email,
                Cpf = model.Cpf,
                UserName = model.Nome,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(barbeiro, model.Senha);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("Admin/CriarPerfil")]
        public async Task<ActionResult> CriarPerfil(string perfil) 
        {
            bool perfilExiste = await _roleManager.RoleExistsAsync(perfil);

            if (perfilExiste) return BadRequest("Perfil existente.");
            
            IdentityRole novaRole = new IdentityRole(perfil);
            var result = await _roleManager.CreateAsync(novaRole);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("Admin/ExcluirPerfil")]
        public async Task<ActionResult> DeletarPerfil(string perfil)
        {
            IdentityRole? roleBd = await _roleManager.FindByNameAsync(perfil);

            if (roleBd is null) return BadRequest("Perfil inexistente.");

            var result = await _roleManager.DeleteAsync(roleBd);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }


        [Authorize(Policy = "AdminOnly")]
        [HttpPost("Admin/AssociarPerfilUsuario")]
        public async Task<ActionResult> AssociarPerfilUsuario(string email, string role)
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(email);

            if (usuario is null) return BadRequest("Usuário inexistente.");

            var result = await _userManager.AddToRoleAsync(usuario, role);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }

        
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("Admin/ExcluirUsuario")]
        public async Task<ActionResult> DeletarUsuario(string email)
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(email);

            if (usuario is null) return BadRequest("Usuário inexistente.");

            var result = await _userManager.DeleteAsync(usuario);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new { Erros = erros });
            }

            return NoContent();
        }
        #endregion
    }
}