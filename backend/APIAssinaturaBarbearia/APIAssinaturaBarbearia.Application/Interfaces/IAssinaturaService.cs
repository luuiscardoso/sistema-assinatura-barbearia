using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura> BuscarAssinaturaEspecifica(int id);

        Task<IEnumerable<Assinatura>> BuscarAssinaturas();

        Task ExcluirAssinatura(int id);

        Task<Assinatura> BuscarAssinaturaPorCpfCliente(string cpf);

        Task<IEnumerable<Assinatura>> BuscarAssinaturaPorNomeCliente(string nome);

        Task<IEnumerable<Assinatura>> BuscarAssinaturaPorStatus(bool status);

        Task<IEnumerable<Assinatura>> BuscarAssinaturasPorData(DateTime dataInicio, DateTime dataFinal);
    }
}
