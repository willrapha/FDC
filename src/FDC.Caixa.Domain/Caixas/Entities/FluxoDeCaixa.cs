using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Entities
{
    public class FluxoDeCaixa : Entity<long, FluxoDeCaixa>
    {
        public DateTime Data { get; private set; }
        public decimal Saldo { get; private set; }
        public List<Movimentacao> Movimentacoes = new List<Movimentacao>();

        public FluxoDeCaixa(DateTime data, decimal saldo)
        {
            Data = data;
            Saldo = saldo;
        }

        protected FluxoDeCaixa() { }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
