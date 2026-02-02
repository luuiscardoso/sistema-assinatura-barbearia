using APIAssinaturaBarbearia.Application.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Domain.Entities;
using Newtonsoft.Json;

namespace APIAssinaturaBarbearia.Controllers
{
    //[Authorize]
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
        /// <remarks>O número de registros retornados por pagina é 5.</remarks>
        /// <returns>Conjunto de assinaturas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterTodasAssinaturas(int numeroPagina)
        {
            PaginacaoDTO<Assinatura> paginacaoAssinatura = await _assinaturaService.BuscarAssinaturasAsync(numeroPagina);

            HeaderAppend(paginacaoAssinatura);

            return Ok(paginacaoAssinatura.Registros);
        }

        /// <summary>
        /// Obtém uma assinatura filtrada por CPF do cliente.
        /// </summary>
        /// <param name="cpf">CPF do cliente.</param>
        /// <returns>Objeto assinatura.</returns>
        [HttpGet("customer/cpf")]
        public async Task<ActionResult<Assinatura>> ObterAssinaturaPorCpfCliente(string cpf)
        {
            Assinatura assinatura = await _assinaturaService.BuscarAssinaturaPorCpfClienteAsync(cpf);

            return Ok(assinatura);
        }

        /// <summary>
        /// Obtém uma lista paginada de assinaturas filtradas por um nome de cliente.
        /// </summary>
        /// <param name="nome">Um nome de cliente</param>
        /// <param name="numeroPagina">Número da página que se deseja visualizar</param>
        /// <remarks>A lista de assinaturas pode também retornar várias 
        /// assinaturas que contem parte do nome especificado.</remarks>
        /// <returns>Conjunto de assinaturas</returns>
        [HttpGet("customer/name")]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterAssinaturaPorNomeCliente(string nome, int numeroPagina)
        {
            PaginacaoDTO<Assinatura> paginacaoAssinatura = await _assinaturaService.BuscarAssinaturaPorNomeClienteAsync(nome, numeroPagina);

            HeaderAppend(paginacaoAssinatura);

            return Ok(paginacaoAssinatura.Registros);
        }

        /// <summary>
        /// Obtém uma lista paginada de assinaturas filtradas pelo status de ativação.
        /// </summary>
        /// <param name="status">Status de ativação.</param>
        /// <param name="numeroPagina">Número da página que se deseja visualizar</param>
        /// <returns>Conjunto de assinaturas</returns>
        [HttpGet("customer/status")]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterAssinaturasPorStatus(bool status, int numeroPagina)
        {
            PaginacaoDTO<Assinatura> paginacaoAssinatura = await _assinaturaService.BuscarAssinaturaPorStatusAsync(status, numeroPagina);

            HeaderAppend(paginacaoAssinatura);

            return Ok(paginacaoAssinatura.Registros);
        }

        /// <summary>
        /// Obtém uma lista paginada de assinaturas que a data de inicio esteja dentro do intervalo de datas especificado.
        /// </summary>
        /// <param name="dataInicio">Data inicial do intervalo</param>
        /// <param name="dataFinal">Data final do intervalo</param>
        /// <param name="numeroPagina">Número da página que se deseja visualizar</param>
        /// <returns>Conjunto de assinaturas</returns>
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<Assinatura>>> ObterAssinaturasPorData(DateTime dataInicio, DateTime dataFinal, int numeroPagina)
        {
            PaginacaoDTO<Assinatura> paginacaoAssinatura = await _assinaturaService.BuscarAssinaturasPorDataAsync(dataInicio, dataFinal, numeroPagina);

            HeaderAppend(paginacaoAssinatura);

            return Ok(paginacaoAssinatura.Registros);
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

