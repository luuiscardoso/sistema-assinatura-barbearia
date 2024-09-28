using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssinaturasController : ControllerBase
    {
        private readonly BdContext _context;
        private readonly IMapper _mapper;
        public AssinaturasController(BdContext bdContext, IMapper mapper)
        {
            _context = bdContext;
            _mapper = mapper;
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

            if (!assinaturas.Any()) return NotFound("Não existe nenhuma assinatura cadastrada.");

            return Ok(assinaturas);
        }

        [HttpPost("Criar")]
        public ActionResult<Assinatura> CriarAssinatura(Cliente cliente)
        {
            Assinatura? novaAssinatura = new Assinatura
            {
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddMonths(1),
                Status = true
            };

            _context.Assinaturas.Add(novaAssinatura);
            _context.SaveChanges();

            Cliente? novoCliente = new Cliente(cliente.Cpf, cliente.Nome, novaAssinatura.AssinaturaId);

            novoCliente.Assinatura = novaAssinatura;
            novaAssinatura.Cliente = novoCliente;

            _context.Clientes.Add(novoCliente);
            _context.SaveChanges();

            return Ok(_context.Assinaturas.Include(a => a.Cliente).FirstOrDefault(a => a.AssinaturaId == novaAssinatura.AssinaturaId));
        }


        [HttpPatch("Alterar/{id:int}")]
        public ActionResult<Assinatura> AlterarAssinatura(int id, JsonPatchDocument<AssinaturaUpdateDTO> patchDoc)
        {
            if (patchDoc is null || patchDoc.Operations.Count == 0) return BadRequest("JSON Patch nulo ou vazio.");

            Assinatura? assinaturaBd = _context.Assinaturas.Include(a => a.Cliente).FirstOrDefault(a => a.AssinaturaId.Equals(id));

            AssinaturaUpdateDTO assinaturaDto = _mapper.Map<AssinaturaUpdateDTO>(assinaturaBd);

            patchDoc.ApplyTo(assinaturaDto, ModelState);

            if(!ModelState.IsValid || !TryValidateModel(assinaturaDto)) return BadRequest(ModelState);

            if(assinaturaDto.Cpf != null || assinaturaDto.Nome != null)
            {
                assinaturaBd.Cliente.Cpf = assinaturaDto.Cpf ?? assinaturaBd.Cliente.Cpf;
                assinaturaBd.Cliente.Nome = assinaturaDto.Nome ?? assinaturaBd.Cliente.Nome;
            }

            _mapper.Map(assinaturaDto, assinaturaBd);

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("Deletar/{id:int:min(1)}")]
        public ActionResult ExcluirAssinatura(int id)
        {
            Assinatura? assinatura = _context.Assinaturas.Find(id);

            if (assinatura == null) return BadRequest("Assinatura não encontrada.");

            _context.Assinaturas.Remove(assinatura);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

