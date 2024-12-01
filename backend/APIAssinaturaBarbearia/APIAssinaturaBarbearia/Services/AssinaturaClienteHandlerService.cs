using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Exceptions;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Services
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

        public async Task<Assinatura> RegistrarNovaAssinatura(ClienteCadastroDTO clienteDto)
        {
            //busca todas assinaturas
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.Todos("Cliente");

            //pega assinatura onde ja exista esse cliente
            Assinatura? assinaturaBd = assinaturas.FirstOrDefault(a => a.Cliente.Cpf.Equals(clienteDto.Cpf));

            //se existir lanca excessao
            if (assinaturaBd is not null)
                throw new AlreadyHasSubscriptionException("Esse cliente já possui assinatura.");

            //se nao cria uma nova para persistir
            Assinatura assinatura = new Assinatura()
            {
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddMonths(1),
                Status = true
            };

            //persiste
            _uof.AssinaturaRepository.Criar(assinatura); //add no contexto - "bd", possui id agora

            Cliente cliente = _clienteService.RegistrarCliente(clienteDto, assinatura); // cliente add no bd com assinatura id

            assinatura.Cliente = cliente;
            cliente.Assinatura = assinatura;

            await _uof.Commit();

            return assinatura;
        }

        public async Task ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto)
        {
            if (assinaturaDto.Cpf != null || assinaturaDto.Nome != null)
            {
                assinaturaBd.Cliente.Cpf = assinaturaDto.Cpf ?? assinaturaBd.Cliente.Cpf;
                assinaturaBd.Cliente.Nome = assinaturaDto.Nome ?? assinaturaBd.Cliente.Nome;
            }

            //Assinatura? assinatura = mapper.Map(assinaturaDto, assinaturaBd);
            assinaturaBd.Status = assinaturaDto.Status;
            assinaturaBd.Fim = assinaturaDto.Fim;  

            _uof.AssinaturaRepository.Atualizar(assinaturaBd);
            await _uof.Commit();
        }
    }
}
