using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Infrastructure.Repositories
{
    public class AssinaturaRepository : Repository<Assinatura>, IAssinaturaRepository
    {
        private readonly BdContext _context;

        public AssinaturaRepository(BdContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Assinatura?> ObterPorCpfCliente(string cpf)
        {
            Assinatura? assinatura = await _context.Assinaturas.AsQueryable()
                                                      .Include(a => a.Cliente)
                                                      .FirstOrDefaultAsync(a => a.Cliente.Cpf == cpf);
            return assinatura;
        }


        public async Task<IEnumerable<Assinatura>> ObterPorNomeCliente(string nome)
        {
            IEnumerable<Assinatura> assinaturas = await _context.Assinaturas.AsQueryable()
                                                               .Include(a => a.Cliente)
                                                               .Where(a => a.Cliente.Nome.Contains(nome))
                                                               .ToListAsync();

            return assinaturas;
        }

        public async Task<IEnumerable<Assinatura>> ObterPorStatus(bool status)
        {
            IEnumerable<Assinatura> assinaturas = await _context.Assinaturas.AsQueryable()
                                                                            .Include(a => a.Cliente)
                                                                            .Where(a => a.Status == status)
                                                                            .ToListAsync();

            return assinaturas;
        }

        public async Task<IEnumerable<Assinatura>> ObterPorData(DateTime dataInicio, DateTime dataFinal)
        {
            IEnumerable<Assinatura> assinaturas = await _context.Assinaturas.AsQueryable()
                                                                            .Include(a => a.Cliente)
                                                                            .Where(a => a.Inicio >= dataInicio && a.Inicio <= dataFinal)
                                                                            .OrderBy(a => a.Inicio)
                                                                            .ToListAsync();

            return assinaturas;
        }
    }
}
