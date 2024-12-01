using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using AutoMapper;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface IAssinaturaClienteHandlerService
    {
        Task<Assinatura> RegistrarNovaAssinatura(ClienteCadastroDTO clienteDto);

        Task ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto);
    }
}
