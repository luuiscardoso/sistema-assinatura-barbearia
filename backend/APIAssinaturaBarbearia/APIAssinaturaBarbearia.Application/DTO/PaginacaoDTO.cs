using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.Application.DTO
{
    public class PaginacaoDTO<T> where T : class
    {
        public PaginacaoDTO(IEnumerable<T> colecao, int numeroPagina)
        {
            Paginar(colecao, numeroPagina);
        }
        public int QtdRegistrosPorPagina { get; private set; } = 5;

        public int TotalRegistros { get; set; }

        public int TotalPaginas { get; private set; }

        public int PaginaAtual { get; private set; }

        public bool TemProxima => PaginaAtual < TotalPaginas;

        public bool TemAnterior => PaginaAtual > 1;

        public IEnumerable<T> Registros { get; private set; } = Enumerable.Empty<T>();
        private void Paginar(IEnumerable<T> colecao, int numeroPagina)
        {
            PaginaAtual = numeroPagina;
            TotalRegistros = colecao.Count();
            TotalPaginas = (int)Math.Ceiling((double)colecao.Count() / QtdRegistrosPorPagina);

            var colecaoPaginada = colecao.Skip((numeroPagina - 1) * QtdRegistrosPorPagina)
                                         .Take(QtdRegistrosPorPagina);

            Registros = colecaoPaginada;
        }
    }
}
