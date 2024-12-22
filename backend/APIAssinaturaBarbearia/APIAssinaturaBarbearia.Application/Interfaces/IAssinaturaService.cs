using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura> BuscarAssinaturaEspecifica(int id);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturas(int numeroPagina);

        Task ExcluirAssinatura(int id);

        Task<Assinatura> BuscarAssinaturaPorCpfCliente(string cpf);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturaPorNomeCliente(string nome, int numeroPagina);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturaPorStatus(bool status, int numeroPagina);

        Task<PaginacaoDTO<Assinatura>> BuscarAssinaturasPorData(DateTime dataInicio, DateTime dataFinal, int numeroPagina);
    }
}
