using FDC.Generics.Bus;

namespace FDC.Caixa.Service.Events
{
    public class PessoaFisicaEvent : Event
    {
        public string Email { get; set; }
    }
}
