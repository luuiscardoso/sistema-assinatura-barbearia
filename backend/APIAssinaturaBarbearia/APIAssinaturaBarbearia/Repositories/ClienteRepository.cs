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

        public Cliente Criar(Cliente cliente)
        {
            var result = _context.Clientes.Add(cliente);
            return result.Entity;
        }
    }
}
