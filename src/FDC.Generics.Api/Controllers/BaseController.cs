using FDC.Generics.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FDC.Generics.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        protected readonly IDomainNotificationService<DomainNotification> NotificacaoDeDominio;

        protected BaseController(IDomainNotificationService<DomainNotification> notificacaoDeDominio)
        {
            NotificacaoDeDominio = notificacaoDeDominio;
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]> {
                { "mensagens", NotificacaoDeDominio.GetNotifications().Select(n => n.Value).ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool OperacaoValida()
        {
            return !NotificacaoDeDominio.HasNotifications();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            var notification = new DomainNotification(TipoDeNotificacao.ErroDeApi.ToString(), erro);

            NotificacaoDeDominio.Add(notification);
        }

        protected void LimparErrosProcessamento()
        {
            NotificacaoDeDominio.Clean();
        }
    }
}
