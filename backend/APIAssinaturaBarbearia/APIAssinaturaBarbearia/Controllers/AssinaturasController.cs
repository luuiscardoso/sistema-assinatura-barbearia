using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using Newtonsoft.Json;
using APIAssinaturaBarbearia.Domain.DTO;

namespace APIAssinaturaBarbearia.Controllers
{
    [Authorize]
    [ApiController]
    [Route("subscriptions")]
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

        /// <summary>
        /// Obtém uma assinatura por ID.
        /// </summary>
        /// <param name="id">ID da assinatura desejada.</param>
        /// <returns>Objeto assinatura</returns>
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorId(int id)
        {
            Assinatura? assinatura = await _assinaturaService.BuscarAssinaturaEspecificaAsync(id);

            return Ok(assinatura);
        }

        /// <summary>
        /// Obtém uma lista paginada de assinaturas.
        /// </summary>
        /// <param name="numeroPagina">Número da página que se deseja visualizar</param>
        /// <remarks>O número de registros retornados por pagina é 10.</remarks>
        /// <returns>Conjunto de assinaturas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterTodasAssinaturas(int numeroPagina)
        {
            PaginacaoDTO<Assinatura> paginacaoAssinatura = await _assinaturaService.BuscarAssinaturasAsync(numeroPagina);

            HeaderAppend(paginacaoAssinatura);

            return Ok(paginacaoAssinatura);
        }

        /// <summary>
        /// Realiza uma busca customizada de assinaturas com filtros opcionais.
        /// </summary>
        /// <remarks>
        /// Endpoint destinado à pesquisa filtrada.
        /// 
        /// Filtros disponíveis (todos opcionais setando null, exceto PageNumber):
        /// - CPF
        /// - Nome
        /// - E-mail
        /// - Intervalo de datas
        /// 
        /// Caso nenhum filtro seja informado, retorna os registros paginados.
        /// </remarks>
        /// <param name="searchFilterDTO">Objeto contendo os filtros de busca via query string.</param>
        /// <returns>Lista paginada de assinaturas.</returns>
        [HttpGet("customers/search")]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterAssinaturaPorBuscaCustomizada([FromQuery] SearchFilterDTO searchFilterDTO)
        {
            var assinaturas = await _assinaturaService.FiltrarAssinaturas(searchFilterDTO);

            return Ok(assinaturas);
        }

        /// <summary>
        /// Cria uma assinatura baseada no nome e CPF de um cliente.
        /// </summary>
        /// <param name="clienteDto">Dados do cliente</param>
        /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     POST Assinaturas/Criar
        ///     {
        ///         "cpf": "94399629822",
        ///         "nome": "string"
        ///     }
        /// </remarks>
        /// <returns>Assinatura criada</returns>
        [HttpPost()]
        public async Task<ActionResult> CriarAssinatura(ClienteCadastroDTO clienteDto)
        {
            var assinaturaCriada = await _assinaturaClienteHandlerService.RegistrarNovaAssinaturaAsync(clienteDto);

            return Created($"Assinaturas/{assinaturaCriada.AssinaturaId}", assinaturaCriada);
        }

        /// <summary>
        /// Altera parcialmente uma assinatura baseada em operações PATCH.
        /// </summary>
        /// <param name="id">ID da assinatura a ser alterada</param>
        /// <param name="patchDoc">JSON Patch com as operações para cada propriedade</param>
        ///  /// <remarks>
        /// Exemplo de request: 
        /// 
        ///     PATCH Assinaturas/Alterar/1
        ///     [
        ///         {
        ///             "path": "/status",
        ///             "op": "replace",
        ///             "value": false
        ///         }
        ///     ]
        ///     
        /// Propriedades passiveis de alteração: status,nome do cliente, cpf do cliente e data de expiração. 
        /// </remarks>
        [HttpPatch("{id:int:min(1)}")]
        public async Task<ActionResult<Assinatura>> AlterarAssinatura(int id, JsonPatchDocument<AssinaturaUpdateDTO> patchDoc)
        {
            if (patchDoc is null || patchDoc.Operations.Count == 0) return BadRequest("JSON Patch nulo ou vazio.");

            Assinatura? assinaturaBd = await _assinaturaService.BuscarAssinaturaEspecificaAsync(id);

            AssinaturaUpdateDTO assinaturaDto = _mapper.Map<AssinaturaUpdateDTO>(assinaturaBd);

            patchDoc.ApplyTo(assinaturaDto, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(assinaturaDto)) return BadRequest(ModelState);

            await _assinaturaClienteHandlerService.ProcessarAtualizacaoAssinaturaAsync(assinaturaBd, assinaturaDto);

            return NoContent();
        }

        /// <summary>
        /// Exclui uma assinatura.
        /// </summary>
        /// <param name="id">ID da assinatura a ser exluida.</param>
        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> ExcluirAssinatura(int id)
        {
            await _assinaturaService.ExcluirAssinaturaAsync(id);
            return NoContent();
        }

        private void HeaderAppend(PaginacaoDTO<Assinatura> paginacao)
        {
            Response.Headers.Append("Paginacao", JsonConvert.SerializeObject(new
            {
                TotalPaginas = paginacao.TotalPaginas,
                PaginaAtual = paginacao.PaginaAtual,
                TemProxima = paginacao.TemProxima,
                TemAnterior = paginacao.TemAnterior,
                QtdRegistrosAtual = paginacao.Registros.Count(),
                TotalRegistros = paginacao.TotalRegistros
            }));
        }
    }
}

