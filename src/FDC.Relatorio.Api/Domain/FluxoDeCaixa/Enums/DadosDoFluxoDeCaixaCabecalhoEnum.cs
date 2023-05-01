using System.ComponentModel;

namespace FDC.Relatorio.Domain.FluxoDeCaixa.Enums
{
    public enum DadosDoFluxoDeCaixaCabecalhoEnum
    {
        [Description("Data")]
        DataHora = 1,
        [Description("Descrição")]
        Descricao = 2,
        [Description("Valor")]
        Valor = 3,
        [Description("Tipo")]
        Tipo = 4
    }
}
