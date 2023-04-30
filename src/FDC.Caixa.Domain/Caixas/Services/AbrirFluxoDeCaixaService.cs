using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Services
{
    public class AbrirFluxoDeCaixaService : DomainService, IAbrirFluxoDeCaixaService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AbrirFluxoDeCaixaService(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IFluxoDeCaixaRepository fluxoDeCaixaRepository,
            IUnitOfWork unitOfWork)
            : base(notificacaoDeDominio)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AbrirFluxoDeCaixa()
        {
            var fluxoDeCaixa = new FluxoDeCaixa(DateTime.Now, SituacaoEnum.Aberto);

            if (!fluxoDeCaixa.Validar())
            {
                NotificarValidacoesDeDominio(fluxoDeCaixa.ValidationResult);
                return;
            }

            await _fluxoDeCaixaRepository.AdicionarAsync(fluxoDeCaixa);

            await _unitOfWork.Commit();
        }
    }
}
