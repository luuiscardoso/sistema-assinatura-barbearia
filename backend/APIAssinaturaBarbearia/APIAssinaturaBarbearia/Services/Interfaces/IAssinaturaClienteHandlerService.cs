using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface IAssinaturaClienteHandlerService
    {
        Task RegistrarNovaAssinatura(ClienteDTO clienteDto);

        void ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto);
    }
}
