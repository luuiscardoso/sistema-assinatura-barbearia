namespace APIAssinaturaBarbearia.Domain.DTO;

public class PaginacaoDTO<T> where T : class
{
    public PaginacaoDTO(IEnumerable<T> colecao, int numeroPagina)
    {
        Paginar(colecao, numeroPagina);
    }

    public PaginacaoDTO(
        IEnumerable<T> registrosPaginados,
        int numeroPagina,
        int totalRegistros,
        int qtdRegistrosPorPagina = 10)
    {
        Paginar(registrosPaginados, numeroPagina, totalRegistros, qtdRegistrosPorPagina);
    }
    public int QtdRegistrosPorPagina { get; set; } = 10;

    public int TotalRegistros { get; set; }

    public int TotalPaginas { get; set; }

    public int PaginaAtual { get; set; }

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

    private void Paginar(
        IEnumerable<T> registrosPaginados,
        int numeroPagina,
        int totalRegistros,
        int qtdRegistrosPorPagina)
    {
        QtdRegistrosPorPagina = qtdRegistrosPorPagina;
        PaginaAtual = numeroPagina;
        TotalRegistros = totalRegistros;
        TotalPaginas = (int)Math.Ceiling((double)totalRegistros / qtdRegistrosPorPagina);
        Registros = registrosPaginados;
    }
}
