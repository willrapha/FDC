using FDC.Generics.Domain;

namespace FDC.Caixa.Infra.Data.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FluxoDeCaixaContext _fluxoDeCaixaContext;

        public UnitOfWork(
            FluxoDeCaixaContext fluxoDeCaixaContext)
        {
            _fluxoDeCaixaContext = fluxoDeCaixaContext;
        }

        public async Task Commit()
        {
            await _fluxoDeCaixaContext.SaveChangesAsync();
        }
    }
}
