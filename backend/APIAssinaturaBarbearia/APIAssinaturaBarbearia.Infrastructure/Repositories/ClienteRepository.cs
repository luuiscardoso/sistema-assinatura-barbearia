using APIAssinaturaBarbearia.Domain.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Infrastructure.Data;

namespace APIAssinaturaBarbearia.Infrastructure.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        private readonly BdContext _context;
        public ClienteRepository(BdContext context) : base(context)
        {
            _context = context;
        }

        public Cliente Criar(Cliente cliente)
        {
            var result = _context.Clientes.Add(cliente);
            return result.Entity;
        }
    }
}
