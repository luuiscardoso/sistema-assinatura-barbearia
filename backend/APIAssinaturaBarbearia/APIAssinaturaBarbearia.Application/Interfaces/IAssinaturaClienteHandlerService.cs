using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaClienteHandlerService
    {
        Task<Assinatura> RegistrarNovaAssinaturaAsync(ClienteCadastroDTO clienteDto);

        Task ProcessarAtualizacaoAssinaturaAsync(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto);
    }
}
