using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using AutoMapper;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface IAssinaturaClienteHandlerService
    {
        Task RegistrarNovaAssinatura(ClienteDTO clienteDto);

        void ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto);
    }
}
