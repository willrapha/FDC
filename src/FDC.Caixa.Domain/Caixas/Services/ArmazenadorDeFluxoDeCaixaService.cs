using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Services
{
    public class ArmazenadorDeFluxoDeCaixaService : DomainService, IArmazenadorDeFluxoDeCaixaService
    {
        public ArmazenadorDeFluxoDeCaixaService(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio) 
            : base(notificacaoDeDominio)
        {
        }


    }
}
