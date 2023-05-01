using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FDC.Generics.Domain;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Dtos;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Enums;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Helpers;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Interfaces;
using FDC.Relatorio.Resources;

namespace FDC.Relatorio.Domain.GeradorDeRelatorio.Services
{
    public class GeradorDeRelatorioService : DomainService, IGeradorDeRelatorioService
    {
        public GeradorDeRelatorioService(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio
            ) : base(notificacaoDeDominio)
        {
        }

        public byte[] GerarRelatorioEmExcel(DadosParaRelatorioDto dados)
        {
            var stream = new MemoryStream();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                var cabecalhoOrdenado = dados.Cabecalho
                    .OrderBy(d => d.Ordem)
                    .ToList();

                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GeradorDeEstiloHelper.RetornarEstiloDasCelulas();
                stylePart.Stylesheet.Save();

                CriarColunas(cabecalhoOrdenado, worksheetPart);

                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = Constantes.UmUint,
                    Name = dados.NomeDoRelatorio
                };

                sheets.Append(sheet);
                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                MontarLinhaDoCabecalho(cabecalhoOrdenado, dados.EstiloHeader, sheetData);
                MontarRegistros(cabecalhoOrdenado, dados.Registros, dados.EstiloRegistro, sheetData);

                workbookPart.Workbook.Save();
                spreadsheetDocument.Close();
            }

            return stream.ToArray();
        }

        private void CriarColunas(List<CabecalhoDto> cabecalhoOrdenado, WorksheetPart worksheetPart)
        {
            var columns = new Columns();

            uint index = 1;

            foreach (var colunaCabecalho in cabecalhoOrdenado)
            {
                var colunaCustomizada = new Column
                {
                    Min = index,
                    Max = index,
                    Width = colunaCabecalho.TamanhoDoCampo,
                    CustomWidth = true
                };

                columns.Append(colunaCustomizada);

                index++;
            }

            worksheetPart.Worksheet.AppendChild(columns);
        }

        private void MontarLinhaDoCabecalho(List<CabecalhoDto> cabecalho, EstiloHeaderEnum estiloHeader, SheetData sheetData)
        {
            Row linhaExcel = new Row() { RowIndex = Constantes.UmUint };

            foreach (var registro in cabecalho)
            {
                var coluna = RetornarColuna(registro.Ordem, Constantes.UmUint);
                var celula = MontarCelula(coluna, new CellValue(registro.NomeDoCampo), CellValues.String, (uint)estiloHeader.GetHashCode());

                linhaExcel.Append(celula);
            }

            sheetData.Append(linhaExcel);
        }

        private void MontarRegistros(List<CabecalhoDto> cabecalho, List<object> registros, EstiloRegistroEnum estiloRegistro, SheetData sheetData)
        {
            uint linha = Constantes.DoisUint;

            foreach (var registroLinha in registros)
            {
                var linhaExcel = new Row { RowIndex = linha };

                foreach (var coluna in cabecalho)
                {
                    var posicaoDaCelula = RetornarColuna(coluna.Ordem, linha);
                    var valorDoCampo = RetornarValorDoCampoDoObjeto(registroLinha, coluna.CampoVinculadoAEntidade);

                    linhaExcel.Append(MontarCelula(posicaoDaCelula, new CellValue(valorDoCampo), CellValues.String, (uint)estiloRegistro.GetHashCode()));
                }

                sheetData.Append(linhaExcel);

                linha++;
            }
        }

        private Cell MontarCelula(string posicaoDaCelula, CellValue valor, CellValues tipoDeDadoDaCelula, uint estiloDaCelula)
        {
            var celula = new Cell
            {
                CellReference = posicaoDaCelula,
                CellValue = valor,
                DataType = tipoDeDadoDaCelula,
                StyleIndex = estiloDaCelula
            };

            return celula;
        }

        private string RetornarColuna(int ordemDaColuna, uint linha)
        {
            var coluna = (ColunaExcelEnum)ordemDaColuna;
            return coluna.GetEnumDescription() + linha;
        }

        private string RetornarValorDoCampoDoObjeto(object objeto, string campo)
        {
            var valorDoCampo = objeto.GetType().GetProperty(campo).GetValue(objeto, null);

            if (valorDoCampo == null)
                return string.Empty;

            return valorDoCampo.ToString();
        }
    }
}
