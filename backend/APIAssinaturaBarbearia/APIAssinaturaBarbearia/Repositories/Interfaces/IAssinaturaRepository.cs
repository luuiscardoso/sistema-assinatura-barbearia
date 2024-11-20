using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Repositories.Interfaces
{
    public interface IAssinaturaRepository : IRepository<Assinatura>
    { 
    }
}
