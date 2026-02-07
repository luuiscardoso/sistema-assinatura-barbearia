using APIAssinaturaBarbearia.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<string>> CriarUsuarioAsync(UsuarioCadastroDTO usuarioCadastroDTO);
        Task<IEnumerable<string>> DeletarUsuarioAsync(string email);
        Task<IEnumerable<string>> CriarPerfilAsync(string perfil);
        Task<IEnumerable<string>> DeletarPerfilAsync(string perfil);
        Task<IEnumerable<string>> AssociarPerfilUsuarioAsync(string email, string role);
    }
}
