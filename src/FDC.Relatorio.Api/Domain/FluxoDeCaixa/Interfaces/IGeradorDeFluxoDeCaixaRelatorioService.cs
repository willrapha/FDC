using FDC.Generics.Domain;
using FDC.Relatorio.Api.Domain.FluxoDeCaixa.Dtos;

namespace FDC.Relatorio.Domain.FluxoDeCaixa.Interfaces
{
    public interface IGeradorDeFluxoDeCaixaRelatorioService
    {
        Task<ArquivoDto> Gerar(FluxoDeCaixaRelatorioDto fluxoDeCaixa);
    }
}
