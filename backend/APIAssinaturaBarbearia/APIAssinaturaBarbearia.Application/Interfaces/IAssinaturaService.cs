using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura> BuscarAssinaturaEspecificaAsync(int id);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturasAsync(int numeroPagina);

        Task ExcluirAssinaturaAsync(int id);

        Task<Assinatura> BuscarAssinaturaPorCpfClienteAsync(string cpf);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturaPorNomeClienteAsync(string nome, int numeroPagina);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturaPorStatusAsync(bool status, int numeroPagina);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturasPorDataAsync(DateTime dataInicio, DateTime dataFinal, int numeroPagina);
    }
}
