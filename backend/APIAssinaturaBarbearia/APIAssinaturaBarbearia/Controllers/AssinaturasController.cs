using APIAssinaturaBarbearia.Application.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;

namespace APIAssinaturaBarbearia.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class AssinaturasController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAssinaturaService _assinaturaService;
        private readonly IAssinaturaClienteHandlerService _assinaturaClienteHandlerService;
        public AssinaturasController(IAssinaturaService assinaturaService, IMapper mapper, IAssinaturaClienteHandlerService assinaturaClienteHandlerService)
        {
            _assinaturaService = assinaturaService;
            _assinaturaClienteHandlerService = assinaturaClienteHandlerService;
            _mapper = mapper;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorId(int id)
        {
            Assinatura? assinatura = await _assinaturaService.BuscarAssinaturaEspecifica(id);

            return Ok(assinatura);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterTodasAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = await _assinaturaService.BuscarAssinaturas();

            return Ok(assinaturas);
        }

        [HttpGet("ObterPorCpfCliente")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorCpfCliente(string cpf)
        {
            Assinatura assinatura = await _assinaturaService.BuscarAssinaturaPorCpfCliente(cpf);

            return Ok(assinatura);
        }

        [HttpGet("ObterPorNomeCliente")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorNomeCliente(string nome)
        {
            IEnumerable<Assinatura> assinaturas = await _assinaturaService.BuscarAssinaturaPorNomeCliente(nome);

            return Ok(assinaturas);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterAssinaturasPorStatus(bool status)
        {
            IEnumerable<Assinatura> assinatura = await _assinaturaService.BuscarAssinaturaPorStatus(status);

            return Ok(assinatura);
        }

        [HttpGet("ObterPorData")]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterAssinaturasPorData(DateTime dataInicio, DateTime dataFinal)
        {
            IEnumerable<Assinatura> assinatura = await _assinaturaService.BuscarAssinaturasPorData(dataInicio, dataFinal);

            return Ok(assinatura);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult> CriarAssinatura(ClienteCadastroDTO clienteDto)
        {
            var assinaturaCriada =  await _assinaturaClienteHandlerService.RegistrarNovaAssinatura(clienteDto);

            return Created($"Assinaturas/{assinaturaCriada.AssinaturaId}", assinaturaCriada);
        }


        [HttpPatch("Alterar/{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> AlterarAssinatura(int id, JsonPatchDocument<AssinaturaUpdateDTO> patchDoc)
        {
            if (patchDoc is null || patchDoc.Operations.Count == 0) return BadRequest("JSON Patch nulo ou vazio.");

            Assinatura? assinaturaBd = await _assinaturaService.BuscarAssinaturaEspecifica(id);

            AssinaturaUpdateDTO assinaturaDto = _mapper.Map<AssinaturaUpdateDTO>(assinaturaBd);

            patchDoc.ApplyTo(assinaturaDto, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(assinaturaDto)) return BadRequest(ModelState);

            await _assinaturaClienteHandlerService.ProcessarAtualizacaoAssinatura(assinaturaBd, assinaturaDto);

            return NoContent();
        }

        [HttpDelete("Deletar/{id:int:min(1)}")]
        public async Task<ActionResult> ExcluirAssinatura(int id)
        {
            await _assinaturaService.ExcluirAssinatura(id);
            return NoContent();
        }
    }
}

