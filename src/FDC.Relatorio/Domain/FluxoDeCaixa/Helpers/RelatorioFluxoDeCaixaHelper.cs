using FDC.Relatorio.Domain.FluxoDeCaixa.Dtos;
using FDC.Relatorio.Domain.FluxoDeCaixa.Enums;
using FDC.Relatorio.Resources;

namespace SENAC.Relatorios.Excel.Service.Relatorios.DadosDaTurma.Helpers
{
    public static class RelatorioFluxoDeCaixaHelper
    {
        public static string RetornarCampoVinculadoAColuna(DadosDoFluxoDeCaixaCabecalhoEnum coluna)
        {
            switch (coluna)
            {
                case DadosDoFluxoDeCaixaCabecalhoEnum.DataHora:
                    return nameof(MovimentacaoRelatorioDto.DataHora);

                case DadosDoFluxoDeCaixaCabecalhoEnum.Descricao:
                    return nameof(MovimentacaoRelatorioDto.Descricao);

                case DadosDoFluxoDeCaixaCabecalhoEnum.Tipo:
                    return nameof(MovimentacaoRelatorioDto.Tipo);

                case DadosDoFluxoDeCaixaCabecalhoEnum.Valor:
                    return nameof(MovimentacaoRelatorioDto.Valor);

                default:
                    return string.Empty;
            }
        }

        public static double RetonarTamanhoDoCampo(DadosDoFluxoDeCaixaCabecalhoEnum coluna)
        {
            switch (coluna)
            {
                case DadosDoFluxoDeCaixaCabecalhoEnum.DataHora:
                    return Constantes.VinteDouble;

                case DadosDoFluxoDeCaixaCabecalhoEnum.Descricao:
                    return Constantes.VinteDouble;

                case DadosDoFluxoDeCaixaCabecalhoEnum.Tipo:
                    return Constantes.VinteDouble;

                case DadosDoFluxoDeCaixaCabecalhoEnum.Valor:
                    return Constantes.VinteDouble;

                default:
                    return Constantes.QuinzeDouble;
            }
        }
    }
}
