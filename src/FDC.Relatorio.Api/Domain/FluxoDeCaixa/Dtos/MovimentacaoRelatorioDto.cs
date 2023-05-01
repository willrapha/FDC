namespace FDC.Relatorio.Domain.FluxoDeCaixa.Dtos
{
    public class MovimentacaoRelatorioDto
    {
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
    }
}
