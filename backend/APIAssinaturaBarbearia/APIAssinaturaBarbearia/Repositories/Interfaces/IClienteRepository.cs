using APIAssinaturaBarbearia.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        EntityEntry<Cliente> Criar(Cliente cliente);
    }
}
