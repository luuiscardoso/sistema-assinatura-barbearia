using APIAssinaturaBarbearia.Application.DTO;
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
        public async Task<Assinatura> BuscarAssinaturaEspecifica(int id)
        {
            Assinatura? assinatura = await _uof.AssinaturaRepository.Obter(a => a.AssinaturaId == id, "Cliente");

            if (assinatura is null) 
                throw new ApplicationNotFoundException("Assinatura não encontrada.");

            return assinatura;
        }

        public async Task<PaginacaoDTO<Assinatura>> BuscarAssinaturas(int numeroPagina)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.Todos("Cliente");

            if (!assinaturas.Any())
                throw new ApplicationNotFoundException("Não existe nenhuma assinatura cadastrada.");

            var paginacao = new PaginacaoDTO<Assinatura>(assinaturas, numeroPagina);

            return paginacao;
        }
        public async Task<Assinatura> BuscarAssinaturaPorCpfCliente(string cpf)
        {
            Assinatura? assinatura = await _uof.AssinaturaRepository.ObterPorCpfCliente(cpf);

            if (assinatura is null)
                throw new ApplicationNotFoundException("Assinatura não encontrada");

            return assinatura;
        }

        public async Task<PaginacaoDTO<Assinatura>> BuscarAssinaturaPorNomeCliente(string nome, int numeroPagina)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.ObterPorNomeCliente(nome);

            if (!assinaturas.Any())
                throw new ApplicationNotFoundException("Não existem assinaturas com esse nome.");

            var paginacao = new PaginacaoDTO<Assinatura>(assinaturas, numeroPagina);

            return paginacao;
        }

        public async Task<PaginacaoDTO<Assinatura>> BuscarAssinaturaPorStatus(bool status, int numeroPagina)
        {
            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.ObterPorStatus(status);

            if (!assinaturas.Any())
                throw new ApplicationNotFoundException("Não existe nenhuma assinatura com esse status.");

            var paginacao = new PaginacaoDTO<Assinatura>(assinaturas, numeroPagina);

            return paginacao;
        }

        public async Task<PaginacaoDTO<Assinatura>> BuscarAssinaturasPorData(DateTime dataInicio, DateTime dataFinal, int numeroPagina)
        {
            if (dataInicio >= dataFinal)
                throw new ApplicationSearchPeriodOfInvalidDatesException("A data de inicio da busca não pode ser anterior ou igual a data final.");

            IEnumerable<Assinatura> assinaturas = await _uof.AssinaturaRepository.ObterPorData(dataInicio, dataFinal);

            if (!assinaturas.Any())
                throw new ApplicationNotFoundException("Não existe nenhuma assinatura nesse intervalo de datas.");

            var paginacao = new PaginacaoDTO<Assinatura>(assinaturas, numeroPagina);

            return paginacao;
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
