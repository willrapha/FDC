namespace FDC.Caixa.Domain.Caixas.Dtos
{
    public class FluxoDeCaixaImprimirDto
    {
        public long Id { get; set; }
        public virtual List<MovimentacaoImprimirDto> Movimentacoes { get; set; } = new List<MovimentacaoImprimirDto>();
    }
}
