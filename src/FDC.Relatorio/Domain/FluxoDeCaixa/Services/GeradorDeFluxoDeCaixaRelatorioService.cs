using FDC.Generics.Domain;
using FDC.Relatorio.Api.Domain.FluxoDeCaixa.Dtos;
using FDC.Relatorio.Domain.FluxoDeCaixa.Enums;
using FDC.Relatorio.Domain.FluxoDeCaixa.Interfaces;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Dtos;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Enums;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Interfaces;
using FDC.Relatorio.Resources;
using SENAC.Relatorios.Excel.Service.Relatorios.DadosDaTurma.Helpers;

namespace FDC.Relatorio.Domain.FluxoDeCaixa.Services
{
    public class GeradorDeFluxoDeCaixaRelatorioService : DomainService, IGeradorDeFluxoDeCaixaRelatorioService
    {
        private readonly IGeradorDeRelatorioService _geradorDeRelatorioService;

        public GeradorDeFluxoDeCaixaRelatorioService(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio, 
            IGeradorDeRelatorioService geradorDeRelatorioService) : base(notificacaoDeDominio)
        {
            _geradorDeRelatorioService = geradorDeRelatorioService;
        }

        public async Task<ArquivoDto> Gerar(FluxoDeCaixaRelatorioDto fluxoDeCaixa)
        {
            try
            {
                var dto = MontarDadosParaRelatorioDto(fluxoDeCaixa);

                return new ArquivoDto
                {
                    Arquivo = _geradorDeRelatorioService.GerarRelatorioEmExcel(dto),
                    ContentTypeXlxs = Constantes.ContentTypeXlxs,
                    Relatorio = Constantes.RelatorioDeDadosDoFluxoDeCaixaXlxs
                };
            }
            catch (Exception ex)
            {
                NotificarValidacaoDominio(ex.Message);
                return null;
            }
        }

        private DadosParaRelatorioDto MontarDadosParaRelatorioDto(FluxoDeCaixaRelatorioDto fluxoDeCaixa)
        {
            var colunasDoCabecalho = MontarNomeDasColunasDoRelatorio();

            return new DadosParaRelatorioDto
            {
                Cabecalho = colunasDoCabecalho,
                NomeDoRelatorio = "Fluxo de Caixa",
                Registros = fluxoDeCaixa.Movimentacoes.Cast<object>().ToList(),
                EstiloHeader = EstiloHeaderEnum.HeaderEmNegritoFundoVerdeLetraBranca,
                EstiloRegistro = EstiloRegistroEnum.RegistroComCorda
            };
        }

        private List<CabecalhoDto> MontarNomeDasColunasDoRelatorio()
        {
            var colunas = new List<CabecalhoDto>();

            var totalDeColunas = Enum.GetNames(typeof(DadosDoFluxoDeCaixaCabecalhoEnum)).Length;

            for (var indiceColuna = Constantes.Um; indiceColuna <= totalDeColunas; indiceColuna++)
            {
                var colunaCabecalho = MontarCabecalhoDto(indiceColuna);

                colunas.Add(colunaCabecalho);
            }

            return colunas;
        }

        private CabecalhoDto MontarCabecalhoDto(int indiceColuna)
        {
            var colunaEnum = (DadosDoFluxoDeCaixaCabecalhoEnum)indiceColuna;

            return new CabecalhoDto
            {
                CampoVinculadoAEntidade = RelatorioFluxoDeCaixaHelper.RetornarCampoVinculadoAColuna(colunaEnum),
                NomeDoCampo = colunaEnum.GetEnumDescription(),
                Ordem = colunaEnum.GetHashCode(),
                TamanhoDoCampo = RelatorioFluxoDeCaixaHelper.RetonarTamanhoDoCampo(colunaEnum)
            };
        }
    }
}
