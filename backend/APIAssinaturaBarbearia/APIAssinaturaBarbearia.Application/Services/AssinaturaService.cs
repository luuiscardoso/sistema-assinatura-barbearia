using APIAssinaturaBarbearia.Application.Exceptions;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Domain.Interfaces;

namespace APIAssinaturaBarbearia.Application.Services
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
                throw new ApplicationNotFoundException("Assinatura não encontrada.");

            return assinatura;
        }

        public async Task<IEnumerable<Assinatura>> BuscarAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.Todos("Cliente");

            if (!assinaturas.Any())
                throw new ApplicationNotFoundException("Não existe nenhuma assinatura cadastrada.");

            return assinaturas;
        }

        public async Task ExcluirAssinatura(int id)
        {
            Assinatura? assinatura = await BuscarAssinaturaEspecifica(id);

            if (assinatura is null) 
                throw new ApplicationNotFoundException("Assinatura não encontrada.");

            _uof.AssinaturaRepository.Excluir(assinatura);
            await _uof.Commit();
        }
    }
}
