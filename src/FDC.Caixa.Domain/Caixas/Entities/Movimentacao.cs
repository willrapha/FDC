using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Entities
{
    public class Movimentacao : Entity<long, Movimentacao>
    {
        public DateTime Periodo { get; private set; }
        public string Descricao { get; private set; }

        public decimal Valor { get; private set; }
        public Tipo Tipo { get; private set; }
        public long FluxoDeCaixaId { get; private set; }
        public virtual FluxoDeCaixa FluxoDeCaixa { get; private set; }

        public Movimentacao(DateTime periodo, string descricao, decimal valor, Tipo tipo, long fluxoDeCaixaId)
        {
            Periodo = periodo;
            Descricao = descricao;
            Valor = valor;
            Tipo = tipo;
            FluxoDeCaixaId = fluxoDeCaixaId;
        }

        protected Movimentacao() { }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
