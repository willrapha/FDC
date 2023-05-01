using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Resource;
using FDC.Generics.Domain;
using FluentValidation;

namespace FDC.Caixa.Domain.Caixas.Entities
{
    public class FluxoDeCaixa : Entity<long, FluxoDeCaixa>
    {
        public DateTime Data { get; private set; }
        public decimal Saldo => ObterSaldo();
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

        private decimal ObterSaldo()
        {
            if (Movimentacoes.Count == 0)
                return decimal.Zero;

            var debito = CalcularOperacao(TipoEmum.Debito);
            var credito = CalcularOperacao(TipoEmum.Credito);

            return (credito - debito);
        }

        private decimal CalcularOperacao(TipoEmum tipo)
        {
            return Movimentacoes.Where(p => p.Tipo == tipo).Sum(m => m.Valor);
        }

        public override bool Validar()
        {
            RuleFor(r => r.Data)
               .GreaterThanOrEqualTo(DateTime.Today);

            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
