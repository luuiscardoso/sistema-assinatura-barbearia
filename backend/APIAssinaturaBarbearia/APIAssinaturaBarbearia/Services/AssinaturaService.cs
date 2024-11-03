using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Exceptions;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace APIAssinaturaBarbearia.Services
{
    public class AssinaturaService : IAssinaturaService
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public AssinaturaService(IUnityOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }
        public async Task<Assinatura?> BuscarAssinaturaEspecifica(int id)
        {
            Assinatura? assinatura = await _uof.AssinaturaRepository.Obter(a => a.AssinaturaId == id, "Cliente");

            if (assinatura is null) 
                throw new NotFoundException("Assinatura não encontrada.");

            return assinatura;
        }

        public async Task<IEnumerable<Assinatura>> BuscarAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.Todos("Cliente");

            if (!assinaturas.Any())
                throw new NotFoundException("Não existe nenhuma assinatura cadastrada.");

            return assinaturas;
        }

        public async Task RegistrarNovaAssinatura(ClienteDTO clienteDto)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.Todos("Cliente");

            Assinatura? assinatura =  assinaturas.FirstOrDefault(a => a.Cliente.Cpf.Equals(clienteDto.Cpf));

            if (assinatura is not null) 
                throw new AlreadyHasSubscriptionException("Esse cliente já possui assinatura.");

            _uof.AssinaturaRepository.Criar(clienteDto);
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

        public async Task ExcluirAssinatura(int id)
        {
            Assinatura? assinatura = await BuscarAssinaturaEspecifica(id);

            if (assinatura is null) 
                throw new NotFoundException("Assinatura não encontrada.");

            _uof.AssinaturaRepository.Excluir(assinatura);
            await _uof.Commit();
        }
    }
}
