using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Repositories
{
    public class AssinaturaRepository : Repository<Assinatura>, IAssinaturaRepository
    {
        private readonly BdContext _context;

        public AssinaturaRepository(BdContext context) : base(context) 
        {
            _context = context;
        }

        //public void Criar(Assinatura assinatura)
        //{
        //    _context.Assinaturas.Add(assinatura);

        //    //EntityEntry<Assinatura> r = _context.Assinaturas.Add(assinatura);
        //    //r.Entity.AssinaturaId;

            

        //    //assinatura.Cliente = novoCliente;
        //    //novoCliente.Assinatura = assinatura;

        //    //_context.Clientes.Add(novoCliente);
        //}
    }
}
