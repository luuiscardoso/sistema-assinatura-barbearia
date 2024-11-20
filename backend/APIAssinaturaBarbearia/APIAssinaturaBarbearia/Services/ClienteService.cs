using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnityOfWork _uof;

        public ClienteService(IUnityOfWork uof)
        {
            _uof = uof;
        }

        public Cliente RegistrarCliente(ClienteDTO clienteDto, Assinatura assinaturaCriada)
        {
            Cliente novoCliente = new Cliente()
            {
                AssinaturaId = assinaturaCriada.AssinaturaId,
                Cpf = clienteDto.Cpf,
                Nome = clienteDto.Nome
            };

           return _uof.ClienteRepository.Criar(novoCliente);
        }
    }
}
