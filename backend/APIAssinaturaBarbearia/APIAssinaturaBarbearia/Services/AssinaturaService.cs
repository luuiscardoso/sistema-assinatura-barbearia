using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Exceptions;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Services
{
    public class AssinaturaService : IAssinaturaService
    {
        private readonly IUnityOfWork _uof;

        public AssinaturaService(IUnityOfWork uof)
        {
            _uof = uof;
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
