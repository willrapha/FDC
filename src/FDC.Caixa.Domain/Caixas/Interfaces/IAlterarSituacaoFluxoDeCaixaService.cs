using FDC.Caixa.Domain.Caixas.Dtos;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IAlterarSituacaoFluxoDeCaixaService
    {
        Task Alterar(FluxoDeCaixaSituacaoDto dto);
    }
}
