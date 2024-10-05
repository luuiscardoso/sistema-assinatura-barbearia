using APIAssinaturaBarbearia.Models;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IAssinaturaRepository : IRepository<Assinatura>
    {
        void Criar(Cliente cliente);
    }
}
