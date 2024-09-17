using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssinaturasController : ControllerBase
    {
        private readonly BdContext _context;
        public AssinaturasController(BdContext bdContext)
        {
            _context = bdContext;
        }
        // /assinaturas/id
        [HttpGet("{id:int}")]
        public ActionResult<Assinatura> ObterAssinaturaPorId(int id)
        {
            if (_context.Assinaturas.Find(id) == null) return NotFound("Assinatura não encontrada.");

            Assinatura? assinatura = _context.Assinaturas.Include(a => a.Cliente).FirstOrDefault(a => a.AssinaturaId == id);

            return Ok(assinatura);
        }

        // /assinaturas
        [HttpGet]
        public ActionResult<IEnumerable<Assinatura>> ObterTodasAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = _context.Assinaturas.Include(a => a.Cliente).ToList();

            if(!assinaturas.Any()) return NotFound("Não existe nenhuma assinatura cadastrada.");

            return Ok(assinaturas);
        }

        [HttpPost("Criar")]
        public ActionResult<Assinatura> CriarAssinatura(Cliente cliente)
        {
            try
            {
                Assinatura? novaAssinatura = new Assinatura(DateTime.Now,
                                                            DateTime.Now.AddMonths(1),
                                                            true);

                _context.Assinaturas.Add(novaAssinatura);
                _context.SaveChanges();

                Cliente? novoCliente = new Cliente(cliente.Cpf, cliente.Nome, novaAssinatura.AssinaturaId);

                novoCliente.Assinatura = novaAssinatura;
                novaAssinatura.Cliente = novoCliente;

                _context.Clientes.Add(novoCliente);
                _context.SaveChanges();

                return Ok(_context.Assinaturas.Include(a => a.Cliente).FirstOrDefault(a => a.AssinaturaId == novaAssinatura.AssinaturaId));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        //[HttpPut("Alterar")]
        //public ActionResult<Assinatura> AlterarAssinatura(Assinatura assinatura)
        //{

        //}
    }
}
