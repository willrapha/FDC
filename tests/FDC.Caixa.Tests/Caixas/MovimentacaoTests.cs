using Bogus;
using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Tests.Base;
using Xunit;

namespace FDC.Caixa.Tests.Caixas
{
    public class MovimentacaoTests
    {
        private readonly Faker _faker;
        private readonly DateTime _dataHora;
        private readonly string _descricao;
        private readonly decimal _valor;
        private readonly TipoEmum _tipo;
        private readonly long _fluxoDeCaixaId;

        public MovimentacaoTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _dataHora = DateTime.Now;
            _descricao = _faker.Lorem.Paragraph();
            _valor = _faker.Random.Decimal(1, 27);
            _tipo = TipoEmum.Debito;
            _fluxoDeCaixaId = _faker.Random.Long(1, 27);
        }

        [Fact]
        public void DeveCriarMovimentacao()
        {
            var movimentacao = new Movimentacao(_dataHora, _descricao, _valor, _tipo, _fluxoDeCaixaId);

            Assert.Equal(_dataHora, movimentacao.DataHora);
            Assert.Equal(_descricao, movimentacao.Descricao);
            Assert.Equal(_valor, movimentacao.Valor);
            Assert.Equal(_tipo, movimentacao.Tipo);
            Assert.Equal(_fluxoDeCaixaId, movimentacao.FluxoDeCaixaId);
        }

        [Theory]
        [InlineData(0)]
        public void DeveValidarMovimentacaoSemFluxo(long fluxoDeCaixaId)
        {
            var movimentacao = new Movimentacao(_dataHora, _descricao, _valor, _tipo, fluxoDeCaixaId);

            var resultado = movimentacao.Validar();

            Assert.False(resultado);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DeveValidarMovimentacaoComValorInvalido(long valor)
        {
            var movimentacao = new Movimentacao(_dataHora, _descricao, valor, _tipo, _fluxoDeCaixaId);

            var resultado = movimentacao.Validar();

            Assert.False(resultado);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeveValidarMovimentacaoComDescricaoInvalida(string descricao)
        {
            var movimentacao = new Movimentacao(_dataHora, descricao, _valor, _tipo, _fluxoDeCaixaId);

            var resultado = movimentacao.Validar();

            Assert.False(resultado);
        }

        [Fact]
        public void DeveValidarMovimentacaoComDataEHoraInvalida()
        {
            var dataHora = _faker.Date.Past();
            var movimentacao = new Movimentacao(dataHora, _descricao, _valor, _tipo, _fluxoDeCaixaId);

            var resultado = movimentacao.Validar();

            Assert.False(resultado);
        }
    }
}
