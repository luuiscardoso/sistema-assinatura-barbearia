using APIAssinaturaBarbearia.Application.Interfaces;
using AutoMapper;
using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Application.DTO;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Application.Exceptions;

namespace APIAssinaturaBarbearia.Application.Services
{
    public class AssinaturaClienteHandlerService : IAssinaturaClienteHandlerService
    {
        private readonly IUnityOfWork _uof;
        private readonly IClienteService _clienteService;

        public AssinaturaClienteHandlerService(IUnityOfWork uof, IClienteService clienteService)
        {
            _uof = uof;
            _clienteService = clienteService;
        }

        public async Task<Assinatura> RegistrarNovaAssinaturaAsync(ClienteCadastroDTO clienteDto)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.TodosAsync("Cliente");

            Assinatura? assinaturaBd = assinaturas.FirstOrDefault(a => a.Cliente.Cpf.Equals(clienteDto.Cpf));

            if (assinaturaBd is not null)
                throw new ApplicationAlreadyHasSubscriptionException("Esse cliente já possui assinatura.");

            Assinatura assinatura = new Assinatura(DateTime.Now, DateTime.Now.AddMonths(1), true);

            _uof.AssinaturaRepository.Criar(assinatura); 

            Cliente cliente = _clienteService.RegistrarCliente(clienteDto, assinatura); 

            assinatura.Cliente = cliente;
            cliente.Assinatura = assinatura;

            await _uof.Commit();

            return assinatura;
        }

        public async Task ProcessarAtualizacaoAssinaturaAsync(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto)
        {
            if (assinaturaDto.Cpf != null || assinaturaDto.Nome != null)
            {
                assinaturaBd.Cliente.Cpf = assinaturaDto.Cpf ?? assinaturaBd.Cliente.Cpf;
                assinaturaBd.Cliente.Nome = assinaturaDto.Nome ?? assinaturaBd.Cliente.Nome;
            }

            assinaturaBd.ValidarAtualizacao(assinaturaDto.Status, assinaturaDto.Fim); 

            _uof.AssinaturaRepository.Atualizar(assinaturaBd);
            await _uof.Commit();
        }
    }
}
