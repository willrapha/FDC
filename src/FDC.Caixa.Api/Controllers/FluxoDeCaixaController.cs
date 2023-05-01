using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Api.Controllers;
using FDC.Generics.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDC.Caixa.Api.Controllers
{
    [Authorize]
    public class FluxoDeCaixaController : BaseController
    {
        private readonly IAlterarSituacaoFluxoDeCaixaService _alterarSituacaoFluxoDeCaixaService;
        private readonly IObterFluxoDeCaixaService _obterFluxoDeCaixaService;
        private readonly IImprimirFluxoDeCaixaService _imprimirFluxoDeCaixaService;

        public FluxoDeCaixaController(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IAlterarSituacaoFluxoDeCaixaService alterarSituacaoFluxoDeCaixaService,
            IObterFluxoDeCaixaService obterFluxoDeCaixaService,
            IImprimirFluxoDeCaixaService imprimirFluxoDeCaixaService) : base(notificacaoDeDominio)
        {
            _alterarSituacaoFluxoDeCaixaService = alterarSituacaoFluxoDeCaixaService;
            _obterFluxoDeCaixaService = obterFluxoDeCaixaService;
            _imprimirFluxoDeCaixaService = imprimirFluxoDeCaixaService;
        }

        [HttpPost("Caixa")]
        public async Task<IActionResult> Caixa(FluxoDeCaixaSituacaoDto dto)
        {
            await _alterarSituacaoFluxoDeCaixaService.Alterar(dto);

            return CustomResponse(new { Caixa = "operacao realizada com sucesso" } );
        }

        [HttpGet("Obter")]
        public async Task<IActionResult> Obter(long id)
        {
            var fluxo = await _obterFluxoDeCaixaService.Obter(id);

            return CustomResponse(fluxo);
        }

        [HttpGet("Imprimir")]
        public async Task<IActionResult> Imprimir(long id)
        {
            var token = HttpContext.Request.Headers["Authorization"];

            var arquivo = await _imprimirFluxoDeCaixaService.Imprimir(id, token);

            return File(arquivo.Arquivo, arquivo.ContentTypeXlxs, arquivo.Relatorio);
        }
    }
}