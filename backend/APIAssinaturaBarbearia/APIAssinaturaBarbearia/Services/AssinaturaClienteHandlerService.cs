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
        private readonly IMapper _mapper;

        public AssinaturaClienteHandlerService(IUnityOfWork uof, IClienteService clienteService, IMapper mapper)
        {
            _uof = uof;
            _clienteService = clienteService;
            _mapper = mapper;
        }

        public async Task RegistrarNovaAssinatura(ClienteDTO clienteDto)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.Todos("Cliente");

            Assinatura? assinaturaBd = assinaturas.FirstOrDefault(a => a.Cliente.Cpf.Equals(clienteDto.Cpf));

            if (assinaturaBd is not null)
                throw new AlreadyHasSubscriptionException("Esse cliente já possui assinatura.");

            Assinatura assinatura = new Assinatura()
            {
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddMonths(1),
                Status = true
            };

            _uof.AssinaturaRepository.Criar(assinatura); //add no contexto - "bd", possui id agora

            EntityEntry<Cliente> cliente = _clienteService.RegistrarCliente(clienteDto, assinatura); // cliente add no bd com assinatura id

            assinatura.Cliente = cliente.Entity;
            cliente.Entity.Assinatura = assinatura;

            await _uof.Commit();
        }

        public void ProcessarAtualizacaoAssinatura(Assinatura assinaturaBd, AssinaturaUpdateDTO assinaturaDto)
        {
            if (assinaturaDto.Cpf != null || assinaturaDto.Nome != null)
            {
                assinaturaBd.Cliente.Cpf = assinaturaDto.Cpf ?? assinaturaBd.Cliente.Cpf;
                assinaturaBd.Cliente.Nome = assinaturaDto.Nome ?? assinaturaBd.Cliente.Nome;
            }

            Assinatura? assinatura = _mapper.Map(assinaturaDto, assinaturaBd);

            _uof.AssinaturaRepository.Atualizar(assinatura);
            _uof.Commit();
        }
    }
}
