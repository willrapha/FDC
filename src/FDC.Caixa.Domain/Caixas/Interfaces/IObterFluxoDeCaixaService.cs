using FDC.Caixa.Domain.Caixas.Dtos;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IObterFluxoDeCaixaService
    {
        Task<FluxoDeCaixaDto> Obter(long id);
    }
}
