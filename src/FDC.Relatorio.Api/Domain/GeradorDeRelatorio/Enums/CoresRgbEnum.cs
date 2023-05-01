using System.ComponentModel;

namespace FDC.Relatorio.Domain.GeradorDeRelatorio.Enums
{
    public enum CoresRgbEnum
    {
        [Description("FFFFFF")]
        Branco = 0,
        [Description("000000")]
        Preto = 1,
        [Description("16B62D")]
        Verde = 2,
        [Description("B4B4B4")]
        CinzaClaro = 3,
    }
}
