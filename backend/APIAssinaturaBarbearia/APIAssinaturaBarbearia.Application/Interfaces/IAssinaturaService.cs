using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura?> BuscarAssinaturaEspecifica(int id);

        Task<IEnumerable<Assinatura>> BuscarAssinaturas();

        Task ExcluirAssinatura(int id);
    }
}
