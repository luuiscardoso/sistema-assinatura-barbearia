using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface IAssinaturaRepository : IRepository<Assinatura>
    {
        Task<Assinatura?> ObterPorCpfClienteAsync(string cpf);
        Task<IEnumerable<Assinatura>> ObterPorNomeClienteAsync(string nome);

        Task<IEnumerable<Assinatura>> ObterPorStatusAsync(bool status);

        Task<IEnumerable<Assinatura>> ObterPorDataAsync(DateTime dataInicio, DateTime dataFinal);
    }
}
