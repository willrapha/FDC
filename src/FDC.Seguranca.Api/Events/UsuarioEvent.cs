using FDC.Generics.Bus;

namespace FDC.Seguranca.Api.Events
{
    public class UsuarioEvent : Event
    {
        public string Email { get; set; }
    }
}
