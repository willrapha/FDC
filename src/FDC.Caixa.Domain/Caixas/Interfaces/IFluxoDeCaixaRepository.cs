using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Generics.Infra;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IFluxoDeCaixaRepository : IRepositorioBase<long, FluxoDeCaixa>
    {
        Task<FluxoDeCaixa> ObterFluxoComMovimentacao(long id);
    }
}
