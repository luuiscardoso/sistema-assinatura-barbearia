using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        private readonly BdContext _context;
        public ClienteRepository(BdContext context) : base(context)
        {
            _context = context;
        }

        public EntityEntry<Cliente> Criar(Cliente cliente)
        {
            return _context.Clientes.Add(cliente);
        }
    }
}
