using FluentValidation.Results;

namespace FDC.Generics.Domain
{
    public abstract class DomainService
    {
        protected readonly IDomainNotificationService<DomainNotification> NotificacaoDeDominio;

        protected DomainService(IDomainNotificationService<DomainNotification> notificacaoDeDominio)
        {
            NotificacaoDeDominio = notificacaoDeDominio;
        }

        public void NotificarValidacoesDeDominio(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
                NotificacaoDeDominio.Add(new DomainNotification(TipoDeNotificacao.ErroDeDominio.ToString(), erro.ErrorMessage));
        }

        public void NotificarValidacaoDominio(string msg)
        {
            NotificacaoDeDominio.Add(new DomainNotification(TipoDeNotificacao.ErroDeDominio.ToString(), msg));
        }
    }
}