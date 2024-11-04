using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface IAssinaturaService
    {
        Task<Assinatura?> BuscarAssinaturaEspecifica(int id);

        Task<IEnumerable<Assinatura>> BuscarAssinaturas();

        Task RegistrarNovaAssinatura (ClienteDTO clienteDto);

        void ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto);

        Task ExcluirAssinatura(int id);
    }
}
