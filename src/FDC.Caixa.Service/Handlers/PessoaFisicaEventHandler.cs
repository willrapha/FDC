using FDC.Caixa.Service.Events;
using FDC.Generics.Bus.Abstractations;

namespace FDC.Caixa.Service.Handlers
{
    public class PessoaFisicaEventHandler : IEventHandler<PessoaFisicaEvent>
    {
        public async Task<bool> Handle(PessoaFisicaEvent @event)
        {
            // TODO: Salvar dados pessoaFisica

            return await Task.Run(() =>
            {
                return true;
            });
        }
    }
}
