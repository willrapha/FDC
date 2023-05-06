using Bogus;
using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Caixa.Domain.Caixas.Services;
using FDC.Caixa.Tests.Base;
using FDC.Caixa.Tests.Builders;
using FDC.Generics.Domain;
using Moq;
using Xunit;

namespace FDC.Caixa.Tests.Caixas
{
    public class ArmazenadorDeMovimentacaoTests
    {
        private readonly Faker _faker;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDomainNotificationService<DomainNotification>> _domainNotificationMock;
        private readonly Mock<IMovimentacaoRepository> _movimentacaoRepositoryMock;
        private readonly Mock<IFluxoDeCaixaRepository> _fluxoDeCaixaRepositoryMock;
        private readonly IArmazenadorDeMovimentacaoService _armazenadorDeMovimentacaoService;

        public ArmazenadorDeMovimentacaoTests()
        {
            _faker = FakerBuilder.Novo().Build();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _domainNotificationMock = new Mock<IDomainNotificationService<DomainNotification>>();
            _movimentacaoRepositoryMock = new Mock<IMovimentacaoRepository>();
            _fluxoDeCaixaRepositoryMock = new Mock<IFluxoDeCaixaRepository>();

            _armazenadorDeMovimentacaoService = new ArmazenadorDeMovimentacaoService(
                _fluxoDeCaixaRepositoryMock.Object,
                _domainNotificationMock.Object,
                _unitOfWorkMock.Object,
                _movimentacaoRepositoryMock.Object);
        }

        [Fact]
        public async Task DeveAdicionarUmaNovaMovimentacao()
        {
            var fluxoDeCaixaId = _faker.Random.Long(1, 27);
            var movimentacao = new AlterarMovimentacaoDto
            {
                Descricao = _faker.Lorem.Paragraph(),
                Valor = _faker.Random.Decimal(1, 27),
                Tipo = TipoEmum.Debito,
                FluxoDeCaixaId = fluxoDeCaixaId
            };
            var fluxoDeCaixa = FluxoDeCaixaBuilder.Novo().ComId(fluxoDeCaixaId).Build();
            _fluxoDeCaixaRepositoryMock.Setup(r => r.ObterPorIdAsync(fluxoDeCaixaId))
                .ReturnsAsync(fluxoDeCaixa);

            await _armazenadorDeMovimentacaoService.Armazenar(movimentacao);

            _movimentacaoRepositoryMock.Verify(repositorio =>
                repositorio.AdicionarAsync(
                    It.Is<Movimentacao>(s => s.Descricao == movimentacao.Descricao && 
                    s.Valor == movimentacao.Valor &&
                    s.Tipo == movimentacao.Tipo &&
                    s.FluxoDeCaixaId == movimentacao.FluxoDeCaixaId)));
        }

        [Fact]
        public async Task NaoDeveAdicionarUmaMovimentacaoInvalida()
        {
            var movimentacaoDto = new AlterarMovimentacaoDto();
            _domainNotificationMock.Setup(notificacao => notificacao.HasNotifications()).Returns(true);

            await _armazenadorDeMovimentacaoService.Armazenar(movimentacaoDto);

            _movimentacaoRepositoryMock.Verify(repositorio =>
                repositorio.AdicionarAsync(It.IsAny<Movimentacao>()), Times.Never());    
        }

        [Fact]
        public async Task DeveNotificarErrosDeDominioQuandoExistir()
        {
            var fluxoDeCaixaId = _faker.Random.Long(1, 27);
            var movimentacao = new AlterarMovimentacaoDto
            {
                Descricao = _faker.Lorem.Paragraph(),
                Tipo = TipoEmum.Credito,
                FluxoDeCaixaId = fluxoDeCaixaId
            };
            var fluxoDeCaixa = FluxoDeCaixaBuilder.Novo().ComId(fluxoDeCaixaId).Build();
            _fluxoDeCaixaRepositoryMock.Setup(r => r.ObterPorIdAsync(fluxoDeCaixaId))
                .ReturnsAsync(fluxoDeCaixa);

            await _armazenadorDeMovimentacaoService.Armazenar(movimentacao);

            _domainNotificationMock.Verify(notificacao =>
                notificacao.Add(It.Is<DomainNotification>(d => d.Key == TipoDeNotificacao.ErroDeDominio.ToString())));
        }

        [Fact]
        public async Task DeveEditarMovimentacao()
        {
            var movimentacaoId = _faker.Random.Long(1, 27);
            var fluxoDeCaixaId = _faker.Random.Long(1, 27);
            var movimentacaoDto = new AlterarMovimentacaoDto
            {
                Id = movimentacaoId,
                Descricao = _faker.Lorem.Paragraph(),
                Tipo = TipoEmum.Credito,
                FluxoDeCaixaId = fluxoDeCaixaId,
                Valor = _faker.Random.Decimal(1, 27)
            };
            var movimentacao = MovimentacaoBuilder.Novo().ComId(movimentacaoId).Build();
            _movimentacaoRepositoryMock.Setup(r => r.ObterPorIdAsync(movimentacaoId))
                .ReturnsAsync(movimentacao);
            var fluxoDeCaixa = FluxoDeCaixaBuilder.Novo().ComId(fluxoDeCaixaId).Build();
            _fluxoDeCaixaRepositoryMock.Setup(r => r.ObterPorIdAsync(fluxoDeCaixaId))
                .ReturnsAsync(fluxoDeCaixa);

            await _armazenadorDeMovimentacaoService.Armazenar(movimentacaoDto);

            Assert.Equal(movimentacaoDto.Descricao, movimentacao.Descricao);
            Assert.Equal(movimentacaoDto.Tipo, movimentacao.Tipo);
            Assert.Equal(movimentacaoDto.Valor, movimentacao.Valor);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
        }
    }
}
