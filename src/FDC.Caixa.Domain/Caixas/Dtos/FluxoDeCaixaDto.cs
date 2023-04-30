using FDC.Caixa.Domain.Caixas.Enums;

namespace FDC.Caixa.Domain.Caixas.Dtos
{
    public class FluxoDeCaixaDto
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Saldo { get; set; }
        public SituacaoEnum Situacao { get; set; }
        public virtual List<MovimentacaoDto> Movimentacoes { get; set; } = new List<MovimentacaoDto>();
    }
}
