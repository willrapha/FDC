namespace FDC.Relatorio.Domain.GeradorDeRelatorio.Dtos
{
    public class CabecalhoDto
    {
        public string NomeDoCampo { get; set; }
        public int Ordem { get; set; }
        public string CampoVinculadoAEntidade { get; set; }
        public double TamanhoDoCampo { get; set; } = 15;
    }
}
