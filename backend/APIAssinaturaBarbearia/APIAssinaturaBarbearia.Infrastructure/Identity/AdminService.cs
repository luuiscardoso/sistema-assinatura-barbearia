using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Application.Exceptions;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace APIAssinaturaBarbearia.Infrastructure.Identity
{
    public class AdminService : IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Usuario> _userManager;
        public AdminService(RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> CriarUsuarioAsync(UsuarioCadastroDTO usuarioCadastroDTO)
        {
            Usuario? barbeiroExiste = await _userManager.FindByEmailAsync(usuarioCadastroDTO.Email);

            if (barbeiroExiste != null) throw new ApplicationUserAlreadyRegisteredException("Usuário já cadastrado");

            Usuario barbeiro = new Usuario()
            {
                Email = usuarioCadastroDTO.Email,
                Cpf = usuarioCadastroDTO.Cpf,
                UserName = usuarioCadastroDTO.Nome,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(barbeiro, usuarioCadastroDTO.Senha);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description);
                return erros;
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> DeletarUsuarioAsync(string email)
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(email);

            if (usuario is null) throw new ApplicationNotFoundException("Usuário inexistente.");

            var result = await _userManager.DeleteAsync(usuario);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description);
                return erros;
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> AssociarPerfilUsuarioAsync(string email, string role)
        {
            Usuario? usuario = await _userManager.FindByEmailAsync(email);

            if (usuario is null) throw new ApplicationNotFoundException("Usuário inexistente.");

            var result = await _userManager.AddToRoleAsync(usuario, role);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(erro => erro.Description);
                return erros;
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> CriarPerfilAsync(string perfil)
        {
            bool perfilExiste = await _roleManager.RoleExistsAsync(perfil);

            if (perfilExiste) throw new ApplicationRoleAlreadyExistsException("Perfil existente.");

            IdentityRole novaRole = new IdentityRole(perfil);
            var result = await _roleManager.CreateAsync(novaRole);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description);
                return erros;
            }

            return Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> DeletarPerfilAsync(string perfil)
        {
            IdentityRole? roleBd = await _roleManager.FindByNameAsync(perfil);

            if (roleBd is null) throw new ApplicationNotFoundException("Perfil inexistente.");

            var result = await _roleManager.DeleteAsync(roleBd);

            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description);
                return erros;
            }

            return Enumerable.Empty<string>();
        }
    }
}
