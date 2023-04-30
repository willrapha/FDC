using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Api.Controllers;
using FDC.Generics.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FDC.Caixa.Api.Controllers
{
    public class MovimentacaoController : BaseController
    {
        private readonly IArmazenadorDeMovimentacaoService _armazenadorDeMovimentacaoService;

        public MovimentacaoController(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IArmazenadorDeMovimentacaoService armazenadorDeMovimentacaoService) : base(notificacaoDeDominio)
        {
            _armazenadorDeMovimentacaoService = armazenadorDeMovimentacaoService;
        }

        [HttpPost("Armazenar")]
        public async Task<IActionResult> Armazenar(MovimentacaoDto movimentacao)
        {
            await _armazenadorDeMovimentacaoService.Armazenar(movimentacao);

            return CustomResponse(new { Movimentacao = "Movimentacao salva com sucesso" } );
        }
    }
}