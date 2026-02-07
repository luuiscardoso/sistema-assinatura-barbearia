using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura> BuscarAssinaturaEspecificaAsync(int id);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturasAsync(int numeroPagina);

        Task ExcluirAssinaturaAsync(int id);

        Task<PaginacaoDTO<Assinatura>> FiltrarAssinaturas(SearchFilterDTO searchFilterDTO);
    }
}
