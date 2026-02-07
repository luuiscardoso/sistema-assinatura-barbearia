using APIAssinaturaBarbearia.Application.Exceptions;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.DTO;
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
        public async Task<Assinatura> BuscarAssinaturaEspecificaAsync(int id)
        {
            Assinatura? assinatura = await _uof.AssinaturaRepository.ObterAsync(a => a.AssinaturaId == id, "Cliente");

            if (assinatura is null) 
                throw new ApplicationNotFoundException("Assinatura não encontrada.");

            return assinatura;
        }

        public async Task<PaginacaoDTO<Assinatura>> BuscarAssinaturasAsync(int numeroPagina)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.TodosAsync("Cliente");

            if (!assinaturas.Any())
                throw new ApplicationNotFoundException("Não existe nenhuma assinatura cadastrada.");

            var paginacao = new PaginacaoDTO<Assinatura>(assinaturas, numeroPagina);

            return paginacao;
        }

        public async Task ExcluirAssinaturaAsync(int id)
        {
            Assinatura? assinatura = await BuscarAssinaturaEspecificaAsync(id);

            if (assinatura is null) 
                throw new ApplicationNotFoundException("Assinatura não encontrada.");

            _uof.AssinaturaRepository.Excluir(assinatura);
            await _uof.Commit();
        }

        public Task<PaginacaoDTO<Assinatura>> FiltrarAssinaturas(SearchFilterDTO searchFilterDTO)
        {
            return _uof.AssinaturaRepository.FiltrarAssinaturas(searchFilterDTO);
        }
    }
}
