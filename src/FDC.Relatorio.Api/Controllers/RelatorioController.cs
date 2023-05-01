using FDC.Generics.Api.Controllers;
using FDC.Generics.Domain;
using FDC.Relatorio.Api.Domain.FluxoDeCaixa.Dtos;
using FDC.Relatorio.Domain.FluxoDeCaixa.Interfaces;
using FDC.Relatorio.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDC.Relatorio.Controllers
{
    [Authorize]
    public class RelatorioController : BaseController
    {
        private readonly IGeradorDeFluxoDeCaixaRelatorioService _geradorDeFluxoDeCaixaRelatorioService;

        public RelatorioController(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio, 
            IGeradorDeFluxoDeCaixaRelatorioService geradorDeFluxoDeCaixaRelatorioService) 
            : base(notificacaoDeDominio)
        {
            _geradorDeFluxoDeCaixaRelatorioService = geradorDeFluxoDeCaixaRelatorioService;
        }

        [HttpPost("ObterRelatorioFluxoDeCaixa")]
        public async Task<IActionResult> ObterRelatorioFluxoDeCaixa(
            [FromBody] FluxoDeCaixaRelatorioDto dto)
        {
            var arquivo = await _geradorDeFluxoDeCaixaRelatorioService.Gerar(dto);

            if (!OperacaoValida())
                return BadRequest();

            return CustomResponse(arquivo);
        }
    }
}
