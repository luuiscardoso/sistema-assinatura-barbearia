﻿using APIAssinaturaBarbearia.Data;
using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services;
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
    [Route("[controller]")]
    public class AssinaturasController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssinaturaService assinaturaService;
        public AssinaturasController(AssinaturaService assinaturaService, IMapper mapper)
        {
            this.assinaturaService = assinaturaService;
            _mapper = mapper;
        }
        // /assinaturas/id
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorId(int id)
        {
            Assinatura? assinatura = await assinaturaService.BuscarAssinaturaEspecifica(id);

            return Ok(assinatura);
        }

        // /assinaturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterTodasAssinaturas()
        {
            IEnumerable<Assinatura> assinaturas = await assinaturaService.BuscarAssinaturas();

            return Ok(assinaturas);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult> CriarAssinatura(ClienteDTO clienteDto)
        {
            await assinaturaService.RegistrarNovaAssinatura(clienteDto);

            return NoContent();
        }


        [HttpPatch("Alterar/{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> AlterarAssinatura(int id, JsonPatchDocument<AssinaturaUpdateDTO> patchDoc)
        {
            if (patchDoc is null || patchDoc.Operations.Count == 0) return BadRequest("JSON Patch nulo ou vazio.");

            Assinatura? assinaturaBd = await assinaturaService.BuscarAssinaturaEspecifica(id);

            AssinaturaUpdateDTO assinaturaDto = _mapper.Map<AssinaturaUpdateDTO>(assinaturaBd);

            patchDoc.ApplyTo(assinaturaDto, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(assinaturaDto)) return BadRequest(ModelState);

            assinaturaService.ProcessarAtualizacaoAssinatura(assinaturaBd, assinaturaDto);

            return NoContent();
        }

        [HttpDelete("Deletar/{id:int:min(1)}")]
        public async Task<ActionResult> ExcluirAssinatura(int id)
        {
            await assinaturaService.ExcluirAssinatura(id);
            return NoContent();
        }
    }
}

