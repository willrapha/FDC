using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Services
{
    public class AlterarSituacaoFluxoDeCaixaService : DomainService, IAlterarSituacaoFluxoDeCaixaService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AlterarSituacaoFluxoDeCaixaService(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IFluxoDeCaixaRepository fluxoDeCaixaRepository,
            IUnitOfWork unitOfWork)
            : base(notificacaoDeDominio)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Alterar(FluxoDeCaixaDto dto)
        {
            var fluxoDeCaixa = new FluxoDeCaixa(DateTime.Now, SituacaoEnum.Aberto);

            if (dto.Id > 0)
            {
                fluxoDeCaixa = await _fluxoDeCaixaRepository.ObterPorIdAsync(fluxoDeCaixa.Id);
                fluxoDeCaixa.AlterarSituacao(dto.Situacao);
            }

            if (!fluxoDeCaixa.Validar())
            {
                NotificarValidacoesDeDominio(fluxoDeCaixa.ValidationResult);
                return;
            }

            if (fluxoDeCaixa.Id == 0)
                await _fluxoDeCaixaRepository.AdicionarAsync(fluxoDeCaixa);

            await _unitOfWork.Commit();
        }
    }
}
