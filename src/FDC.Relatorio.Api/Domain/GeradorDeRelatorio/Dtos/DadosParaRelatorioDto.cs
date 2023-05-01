using FDC.Relatorio.Domain.GeradorDeRelatorio.Enums;

namespace FDC.Relatorio.Domain.GeradorDeRelatorio.Dtos
{
    public class DadosParaRelatorioDto
    {
        public List<CabecalhoDto> Cabecalho { get; set; }
        public List<object> Registros { get; set; }
        public string NomeDoRelatorio { get; set; }
        public EstiloHeaderEnum EstiloHeader { get; set; } = EstiloHeaderEnum.Default;
        public EstiloRegistroEnum EstiloRegistro { get; set; } = EstiloRegistroEnum.Default;
    }
}
