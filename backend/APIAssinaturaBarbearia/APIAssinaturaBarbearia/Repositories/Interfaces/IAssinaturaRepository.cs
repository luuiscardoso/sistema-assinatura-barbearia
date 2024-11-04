using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IAssinaturaRepository : IRepository<Assinatura>
    {
        void Criar(ClienteDTO cliente);
    }
}
