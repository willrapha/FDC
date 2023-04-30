using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Generics.Infra;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IMovimentacaoRepository : IRepositorioBase<long, Movimentacao>
    {
    }
}
