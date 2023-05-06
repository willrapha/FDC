using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Tests.Base;

namespace FDC.Caixa.Tests.Builders
{
    public class MovimentacaoBuilder : BuilderBase
    {
        private long _id;
        private string _descricao;
        private TipoEmum _tipo;
        private long _fluxoDeCaixaId;
        private decimal _valor;
        private DateTime _dataHora;

        public static MovimentacaoBuilder Novo()
        {
            var fake = FakerBuilder.Novo().Build();
            var builder = new MovimentacaoBuilder
            {
                _id = fake.Random.Long(1, 27),
                _descricao = fake.Lorem.Paragraph(),
                _tipo = TipoEmum.Debito,
                _fluxoDeCaixaId = fake.Random.Long(1,27),
                _valor = fake.Random.Decimal(1, 27),
                _dataHora = DateTime.Now
            };

            return builder;
        }

        public MovimentacaoBuilder ComId(long id)
        {
            _id = id;
            return this;
        }

        public MovimentacaoBuilder ComDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public MovimentacaoBuilder ComTipo(TipoEmum tipo)
        {
            _tipo = tipo;
            return this;
        }

        public MovimentacaoBuilder ComFluxoDeCaixa(long fluxoDeCaixaId)
        {
            _fluxoDeCaixaId = fluxoDeCaixaId;
            return this;
        }

        public MovimentacaoBuilder ComValor(decimal valor)
        {
            _valor = valor;
            return this;
        }

        public MovimentacaoBuilder ComDataHora(DateTime dataHora)
        {
            _dataHora = dataHora;
            return this;
        }

        public Movimentacao Build()
        {
            var movimentacao = new Movimentacao(_dataHora, _descricao, _valor, _tipo, _fluxoDeCaixaId);
            AtribuirId(_id, movimentacao);
            return movimentacao;
        }
    }
}
