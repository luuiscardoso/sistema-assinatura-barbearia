using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services;
using APIAssinaturaBarbearia.Services.Interfaces;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIAssinaturaBarbearia.Controllers
{
    [Authorize]
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
        // /assinaturas/id
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorId(int id)
        {
            Assinatura? assinatura = await _assinaturaService.BuscarAssinaturaEspecifica(id);

            return Ok(assinatura);
        }

        // /assinaturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterTodasAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = await _assinaturaService.BuscarAssinaturas();

            return Ok(assinaturas);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult> CriarAssinatura(ClienteCadastroDTO clienteDto)
        {
            var assinaturaCriada =  await _assinaturaClienteHandlerService.RegistrarNovaAssinatura(clienteDto);

            return Created($"Assinaturas/{assinaturaCriada.AssinaturaId}", assinaturaCriada);
        }


        [HttpPatch("Alterar/{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> AlterarAssinatura(int id, [FromBody] JsonPatchDocument<AssinaturaUpdateDTO> patchDoc)
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

