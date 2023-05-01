using DocumentFormat.OpenXml.Spreadsheet;
using FDC.Generics.Domain;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Enums;

namespace FDC.Relatorio.Domain.GeradorDeRelatorio.Helpers
{
    public static class GeradorDeEstiloHelper
    {
        public static Stylesheet RetornarEstiloDasCelulas()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font(new FontSize() { Val = 11 }),                                                                              
                new Font(new FontSize() { Val = 11 }, new Bold()),                                                                  
                new Font(new FontSize() { Val = 11 }, new Bold(), new Color() { Rgb = CoresRgbEnum.Branco.GetEnumDescription() })   
            );

            Fills fills = new Fills(
                new Fill(new PatternFill() { PatternType = PatternValues.None }),                                                                            
                new Fill(new PatternFill(new ForegroundColor { Rgb = CoresRgbEnum.CinzaClaro.GetEnumDescription() }) { PatternType = PatternValues.Solid }), 
                new Fill(new PatternFill(new ForegroundColor { Rgb = CoresRgbEnum.Verde.GetEnumDescription() }) { PatternType = PatternValues.Solid })       

            );

            Borders borders = new Borders(
                new Border(),   
                new Border(     
                    new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            );

            CellFormats cellFormats = new CellFormats(                                                          
                new CellFormat(),                                                                               
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyFill = true, ApplyBorder = true },  
                new CellFormat { FontId = 2, FillId = 1, BorderId = 1, ApplyFill = true, ApplyBorder = true },  
                new CellFormat { FontId = 2, FillId = 2, BorderId = 1, ApplyFill = true, ApplyBorder = true },  
                new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyFill = true, ApplyBorder = true },  
                new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyFill = true, ApplyBorder = true },  
                new CellFormat { FontId = 0, FillId = 0, BorderId = 0, ApplyFill = true, ApplyBorder = true },  
                new CellFormat { FontId = 0, FillId = 0, BorderId = 0, ApplyFill = true, ApplyBorder = true }   
            );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
    }
}
