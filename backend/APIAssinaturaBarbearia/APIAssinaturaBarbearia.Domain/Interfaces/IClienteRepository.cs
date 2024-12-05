using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Domain.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Cliente Criar(Cliente cliente);
    }
}
