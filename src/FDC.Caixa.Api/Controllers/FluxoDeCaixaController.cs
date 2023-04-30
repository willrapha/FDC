using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Api.Controllers;
using FDC.Generics.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FDC.Caixa.Api.Controllers
{
    public class FluxoDeCaixaController : BaseController
    {
        private readonly IAbrirFluxoDeCaixaService _abrirFluxoDeCaixaService;
        private readonly IObterFluxoDeCaixaService _obterFluxoDeCaixaService;

        public FluxoDeCaixaController(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IAbrirFluxoDeCaixaService abrirFluxoDeCaixaService,
            IObterFluxoDeCaixaService obterFluxoDeCaixaService) : base(notificacaoDeDominio)
        {
            _abrirFluxoDeCaixaService = abrirFluxoDeCaixaService;
            _obterFluxoDeCaixaService = obterFluxoDeCaixaService;
        }

        [HttpGet("Abrir")]
        public async Task<IActionResult> Abrir()
        {
            await _abrirFluxoDeCaixaService.AbrirFluxoDeCaixa();

            return CustomResponse(new { Caixa = "caixa aberto com sucesso" } );
        }

        [HttpGet("Obter")]
        public async Task<IActionResult> Obter(long id)
        {
            var fluxo = await _obterFluxoDeCaixaService.Obter(id);

            return CustomResponse(fluxo);
        }
    }
}