using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Caixa.Infra.Data.Context;
using FDC.Generics.Infra;
using Microsoft.EntityFrameworkCore;

namespace FDC.Caixa.Infra.Data.Repositories
{
    public class FluxoDeCaixaRepository : RepositoryBase<long, FluxoDeCaixa>, IFluxoDeCaixaRepository
    {
        private readonly FluxoDeCaixaContext _context;

        public FluxoDeCaixaRepository(FluxoDeCaixaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FluxoDeCaixa> ObterFluxoComMovimentacao(long id)
        {
            return await _context.FluxoDeCaixa
                .Include(_ => _.Movimentacoes)
                .FirstOrDefaultAsync(_ => _.Id == id);
        }
    }
}
