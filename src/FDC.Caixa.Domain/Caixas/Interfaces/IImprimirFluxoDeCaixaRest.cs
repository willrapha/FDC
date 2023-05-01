using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IImprimirFluxoDeCaixaRest
    {
        Task<ArquivoDto> ObterPorId(FluxoDeCaixaImprimirDto dto, string token);
    }
}
