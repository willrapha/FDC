using System.ComponentModel;

namespace FDC.Caixa.Domain.Caixas.Enums
{
    public enum TipoEmum
    {
        [Description("Débito")]
        Debito = 1,
        [Description("Crédito")]
        Credito = 2
    }
}
