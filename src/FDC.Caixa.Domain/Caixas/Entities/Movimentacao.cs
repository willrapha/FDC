using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Generics.Domain;
using FluentValidation;

namespace FDC.Caixa.Domain.Caixas.Entities
{
    public class Movimentacao : Entity<long, Movimentacao>
    {
        public DateTime DataHora { get; private set; }
        public string Descricao { get; private set; }

        public decimal Valor { get; private set; }
        public TipoEmum Tipo { get; private set; }
        public long FluxoDeCaixaId { get; private set; }
        public virtual FluxoDeCaixa FluxoDeCaixa { get; private set; }

        public Movimentacao(DateTime dataHora, string descricao, decimal valor, TipoEmum tipo, long fluxoDeCaixaId)
        {
            DataHora = dataHora;
            Descricao = descricao;
            Valor = valor;
            Tipo = tipo;
            FluxoDeCaixaId = fluxoDeCaixaId;
        }

        protected Movimentacao() { }

        public void AlterarTipo(TipoEmum tipo)
        {
            Tipo = tipo;
        }

        public void AlterarValor(decimal valor)
        {
            Valor = valor;
        }

        public void AlterarDescricao(string descricao)
        {
            Descricao = descricao;
        }

        public override bool Validar()
        {
            RuleFor(r => r.DataHora)
               .GreaterThanOrEqualTo(DateTime.Today);

            RuleFor(r => r.Descricao)
               .NotEmpty()
               .NotNull();

            RuleFor(r => r.Valor)
               .GreaterThan(0);

            RuleFor(r => r.FluxoDeCaixaId)
               .GreaterThan(0);

            ValidationResult = Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
