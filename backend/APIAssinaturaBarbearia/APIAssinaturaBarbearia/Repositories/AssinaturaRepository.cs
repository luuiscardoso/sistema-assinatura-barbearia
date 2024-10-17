using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Repositories
{
    public class AssinaturaRepository : Repository<Assinatura>, IAssinaturaRepository
    {
        private readonly BdContext _context;

        public AssinaturaRepository(BdContext context) : base(context) 
        {
            _context = context;
        }

        public void Criar(Cliente cliente)
        {
            Assinatura assinatura = new Assinatura()
            {
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddMonths(1),
                Status = true
            };

            _context.Assinaturas.Add(assinatura);

            Cliente novoCliente = new Cliente()
            {
                AssinaturaId = assinatura.AssinaturaId,
                Cpf = cliente.Cpf,
                Nome = cliente.Nome
            };

            assinatura.Cliente = novoCliente;
            novoCliente.Assinatura = assinatura;

            _context.Clientes.Add(novoCliente);
        }
    }
}
