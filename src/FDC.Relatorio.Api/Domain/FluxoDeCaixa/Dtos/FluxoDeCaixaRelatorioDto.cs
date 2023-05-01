using FDC.Relatorio.Domain.FluxoDeCaixa.Dtos;

namespace FDC.Relatorio.Api.Domain.FluxoDeCaixa.Dtos
{
    public class FluxoDeCaixaRelatorioDto
    {
        public long Id { get; set; }
        public List<MovimentacaoRelatorioDto> Movimentacoes { get; set; } = new List<MovimentacaoRelatorioDto>();
    }
}
