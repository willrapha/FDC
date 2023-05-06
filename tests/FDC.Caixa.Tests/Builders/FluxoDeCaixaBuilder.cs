using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Tests.Base;

namespace FDC.Caixa.Tests.Builders
{
    public class FluxoDeCaixaBuilder : BuilderBase
    {
        private long _id;
        private DateTime _data;
        private SituacaoEnum _situacao;

        public static FluxoDeCaixaBuilder Novo()
        {
            var fake = FakerBuilder.Novo().Build();
            var builder = new FluxoDeCaixaBuilder
            {
                _id = fake.Random.Long(1, 27),
                _data = DateTime.Now,
                _situacao = SituacaoEnum.Aberto
            };

            return builder;
        }

        public FluxoDeCaixaBuilder ComId(long id)
        {
            _id = id;
            return this;
        }

        public FluxoDeCaixaBuilder ComData(DateTime data)
        {
            _data = data;
            return this;
        }

        public FluxoDeCaixaBuilder ComSituacao(SituacaoEnum situacao)
        {
            _situacao = situacao;
            return this;
        }

        public FluxoDeCaixa Build()
        {
            var fluxoDeCaixa = new FluxoDeCaixa(_data, _situacao);
            AtribuirId(_id, fluxoDeCaixa);
            return fluxoDeCaixa;
        }
    }
}
