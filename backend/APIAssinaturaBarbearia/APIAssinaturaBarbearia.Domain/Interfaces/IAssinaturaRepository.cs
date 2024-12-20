using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface IAssinaturaRepository : IRepository<Assinatura>
    {
        Task<Assinatura?> ObterPorCpfCliente(string cpf);
        Task<IEnumerable<Assinatura>> ObterPorNomeCliente(string nome);

        Task<IEnumerable<Assinatura>> ObterPorStatus(bool status);

        Task<IEnumerable<Assinatura>> ObterPorData(DateTime dataInicio, DateTime dataFinal);
    }
}
