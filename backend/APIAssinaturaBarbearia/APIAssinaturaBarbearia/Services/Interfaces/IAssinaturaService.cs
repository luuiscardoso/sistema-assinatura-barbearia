using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura?> BuscarAssinaturaEspecifica(int id);

        Task<IEnumerable<Assinatura>> BuscarAssinaturas();

        Task ExcluirAssinatura(int id);
    }
}
