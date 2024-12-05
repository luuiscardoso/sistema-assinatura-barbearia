using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Infrastructure.Data;

namespace APIAssinaturaBarbearia.Infrastructure.Repositories
{
    public class AssinaturaRepository : Repository<Assinatura>, IAssinaturaRepository
    {
        private readonly BdContext _context;

        public AssinaturaRepository(BdContext context) : base(context) 
        {
            _context = context;
        }
    }
}
