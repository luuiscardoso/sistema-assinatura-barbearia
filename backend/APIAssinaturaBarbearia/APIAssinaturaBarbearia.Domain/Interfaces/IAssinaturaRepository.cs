using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface IAssinaturaRepository : IRepository<Assinatura>
    {
        Task<PaginacaoDTO<Assinatura>> FiltrarAssinaturas(SearchFilterDTO searchFilterDTO);
    }
}
