using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Repositories
{
    public class AssinaturaRepositorie : IAssinaturaRepositorie
    {
        private readonly BdContext _context;

        public AssinaturaRepositorie(BdContext context)
        {
            _context = context;
        }
        public Assinatura? Obter(int id)
        {
            return _context.Assinaturas.Include(a => a.Cliente)
                                       .FirstOrDefault(a => a.AssinaturaId.Equals(id));
        }
        public IEnumerable<Assinatura> ObterTodas()
        {
            return _context.Assinaturas.Include(a => a.Cliente)
                                       .ToList();
        }
        public void Atualizar(Assinatura assinatura)
        {
            _context.Assinaturas.Update(assinatura);
            _context.SaveChanges();
        }

        public void Criar(Cliente cliente)
        {
            using var transaction = _context.Database.BeginTransaction();

            Assinatura assinatura = new Assinatura()
            {
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddMonths(1),
                Status = true
            };

            _context.Assinaturas.Add(assinatura);
            _context.SaveChanges();

            Cliente novoCliente = new Cliente()
            {
                AssinaturaId = assinatura.AssinaturaId,
                Cpf = cliente.Cpf,
                Nome = cliente.Nome
            };

            assinatura.Cliente = novoCliente;
            novoCliente.Assinatura = assinatura;

            _context.Clientes.Add(novoCliente);
            _context.SaveChanges();

            transaction.Commit();
        }

        public void Excluir(Assinatura assinatura)
        {
            _context.Assinaturas.Remove(assinatura);
            _context.SaveChanges();
        }


    }
}
