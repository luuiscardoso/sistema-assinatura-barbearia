using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories;
using APIAssinaturaBarbearia.Repositories.Interfaces;
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
        private readonly IAssinaturaRepositorie _assinaturaRepository;
        private readonly IMapper _mapper;
        public AssinaturasController(IAssinaturaRepositorie assinaturaRepository, IMapper mapper)
        {
            _assinaturaRepository = assinaturaRepository;
            _mapper = mapper;
        }
        // /assinaturas/id
        [HttpGet("{id:int:min(1)}")]
        public ActionResult<Assinatura> ObterAssinaturaPorId(int id)
        {
            if (_assinaturaRepository.Obter(id) == null) return NotFound("Assinatura não encontrada.");

            Assinatura? assinatura = _assinaturaRepository.Obter(id);

            return Ok(assinatura);
        }

        // /assinaturas
        [HttpGet]
        public ActionResult<IEnumerable<Assinatura>> ObterTodasAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = _assinaturaRepository.ObterTodas();

            if (!assinaturas.Any()) return NotFound("Não existe nenhuma assinatura cadastrada.");

            return Ok(assinaturas);
        }

        [HttpPost("Criar")]
        public ActionResult CriarAssinatura(Cliente cliente)
        {
            IEnumerable<Assinatura> assinaturas = _assinaturaRepository.ObterTodas();

            Assinatura? assinatura = assinaturas.FirstOrDefault(a => a.Cliente.Cpf.Equals(cliente.Cpf))/*Where(a => a.Cliente.Cpf.Equals(cliente.Cpf))*/;
            if (assinatura is not null) return BadRequest("Esse cliente já possui assinatura.");

            _assinaturaRepository.Criar(cliente);
            return NoContent();
        }


        [HttpPatch("Alterar/{id:int:min(1)}")]
        public ActionResult<Assinatura> AlterarAssinatura(int id, JsonPatchDocument<AssinaturaUpdateDTO> patchDoc)
        {
            if (patchDoc is null || patchDoc.Operations.Count == 0) return BadRequest("JSON Patch nulo ou vazio.");

            Assinatura? assinaturaBd = _assinaturaRepository.Obter(id);

            if (assinaturaBd is null) return NotFound("Erro ao alterar. Assinatura inexistente.");

            AssinaturaUpdateDTO assinaturaDto = _mapper.Map<AssinaturaUpdateDTO>(assinaturaBd);

            patchDoc.ApplyTo(assinaturaDto, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(assinaturaDto)) return BadRequest(ModelState);

            if (assinaturaDto.Cpf != null || assinaturaDto.Nome != null)
            {
                assinaturaBd.Cliente.Cpf = assinaturaDto.Cpf ?? assinaturaBd.Cliente.Cpf;
                assinaturaBd.Cliente.Nome = assinaturaDto.Nome ?? assinaturaBd.Cliente.Nome;
            }

            Assinatura? assinatura = _mapper.Map(assinaturaDto, assinaturaBd);

            _assinaturaRepository.Atualizar(assinatura);

            return NoContent();
        }

        [HttpDelete("Deletar/{id:int:min(1)}")]
        public ActionResult ExcluirAssinatura(int id)
        {
            Assinatura? assinatura = _assinaturaRepository.Obter(id);

            if (assinatura == null) return BadRequest("Assinatura não encontrada.");

            _assinaturaRepository.Excluir(assinatura);

            return NoContent();
        }
    }
}

