using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Domain.Interfaces;


namespace APIAssinaturaBarbearia.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnityOfWork _uof;

        public ClienteService(IUnityOfWork uof)
        {
            _uof = uof;
        }

        public Cliente RegistrarCliente(ClienteCadastroDTO clienteDto, Assinatura assinaturaCriada)
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
