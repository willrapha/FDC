using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Services
{
    public class ArmazenarMovimentacaoService : DomainService, IArmazenadorDeMovimentacaoService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArmazenarMovimentacaoService(
            IFluxoDeCaixaRepository fluxoDeCaixaRepository,
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IUnitOfWork unitOfWork,
            IMovimentacaoRepository movimentacaoRepository) : base(notificacaoDeDominio)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
            _unitOfWork = unitOfWork;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task Armazenar(MovimentacaoDto dto)
        {
            var fluxo = await _fluxoDeCaixaRepository.ObterPorIdAsync(dto.FluxoDeCaixaId);

            if (!PermiteLancarMovimentacao(fluxo))
                return;

            var movimentacao = new Movimentacao(DateTime.Now, dto.Descricao, dto.Valor, dto.Tipo, dto.FluxoDeCaixaId);

            if (dto.Id > 0)
            {
                movimentacao = await _movimentacaoRepository.ObterPorIdAsync(dto.Id);
                movimentacao.AlterarDescricao(dto.Descricao);
                movimentacao.AlterarTipo(dto.Tipo);
                movimentacao.AlterarValor(dto.Valor);
            }

            if (!movimentacao.Validar())
            {
                NotificarValidacoesDeDominio(movimentacao.ValidationResult);
                return;
            }

            if (dto.Id == 0)
                await _movimentacaoRepository.AdicionarAsync(movimentacao);

            await _unitOfWork.Commit();
        }

        private bool PermiteLancarMovimentacao(FluxoDeCaixa fluxo)
        {
            if (fluxo == null)
            {
                NotificarValidacaoDominio("Fluxo de caixa não existe");
                return false;
            }

            if (fluxo.Situacao == Enums.SituacaoEnum.Fechado)
                NotificarValidacaoDominio("Caixa esta fechado");

            if (NotificacaoDeDominio.HasNotifications())
                return false;

            return true;
        }
    }
}
