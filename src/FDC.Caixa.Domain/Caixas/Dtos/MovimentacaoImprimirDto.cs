namespace FDC.Caixa.Domain.Caixas.Dtos
{
    public class MovimentacaoImprimirDto
    {
        public DateTime DataHora { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; }
    }
}
