using APIAssinaturaBarbearia.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IUserService
    {
        Task<TokenResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<TokenResponseDTO> RenovarTokenAsync(TokenRequestDTO tokenDTO);
        Task<IEnumerable<string>> RedefinirSenhaAsync(BarbeiroRedefinicaoSenhaDTO model);
        Task RevogarRefreshTokenAsync(string email, ClaimsPrincipal httpContextUser);
    }
}
