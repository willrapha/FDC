using FDC.Relatorio.Domain.GeradorDeRelatorio.Dtos;

namespace FDC.Relatorio.Domain.GeradorDeRelatorio.Interfaces
{
    public interface IGeradorDeRelatorioService
    {
        byte[] GerarRelatorioEmExcel(DadosParaRelatorioDto dados);
    }
}
