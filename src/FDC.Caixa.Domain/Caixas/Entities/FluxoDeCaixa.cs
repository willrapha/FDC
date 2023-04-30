using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Entities
{
    public class FluxoDeCaixa : Entity<long, FluxoDeCaixa>
    {
        public DateTime Data { get; private set; }
        public decimal Saldo => Movimentacoes.Sum(m => m.Valor);
        public SituacaoEnum Situacao { get; private set; }
        public virtual List<Movimentacao> Movimentacoes { get; private set; } = new List<Movimentacao>();

        public FluxoDeCaixa(DateTime data, SituacaoEnum situacao)
        {
            Data = data;
            Situacao = situacao;
        }

        protected FluxoDeCaixa() { }

        public void AlterarSituacao(SituacaoEnum situacao)
        {
            Situacao = situacao;
        }

        public override bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
