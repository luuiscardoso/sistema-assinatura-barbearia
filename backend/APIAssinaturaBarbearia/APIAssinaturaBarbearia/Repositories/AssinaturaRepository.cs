using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
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

        public void Criar(ClienteDTO clienteDto)
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
                Cpf = clienteDto.Cpf,
                Nome = clienteDto.Nome
            };

            assinatura.Cliente = novoCliente;
            novoCliente.Assinatura = assinatura;

            _context.Clientes.Add(novoCliente);
        }
    }
}
