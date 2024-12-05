using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Application.Interfaces
{
    public interface IAssinaturaClienteHandlerService
    {
        Task<Assinatura> RegistrarNovaAssinatura(ClienteCadastroDTO clienteDto);

        Task ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto);
    }
}
