using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Caixa.Infra.Data.Context;
using FDC.Generics.Infra;

namespace FDC.Caixa.Infra.Data.Repositories
{
    public class MovimentacaoRepository : RepositoryBase<long, Movimentacao>, IMovimentacaoRepository
    {
        public MovimentacaoRepository(FluxoDeCaixaContext context) : base(context)
        {
        }
    }
}
