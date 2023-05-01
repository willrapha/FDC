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

        public async Task Alterar(FluxoDeCaixaSituacaoDto dto)
        {
            var fluxoDeCaixa = new FluxoDeCaixa(DateTime.Now, dto.Situacao);

            if (!VerificaSituacaoDoCaixa(dto))
                return;

            await AlterarCaixaExistenteAsync(dto);

            if (!fluxoDeCaixa.Validar())
            {
                NotificarValidacoesDeDominio(fluxoDeCaixa.ValidationResult);
                return;
            }

            if (fluxoDeCaixa.Id == 0)
                await _fluxoDeCaixaRepository.AdicionarAsync(fluxoDeCaixa);

            await _unitOfWork.Commit();
        }

        private bool VerificaSituacaoDoCaixa(FluxoDeCaixaSituacaoDto dto)
        {
            if (dto.Id > 0)
                return true;

            if (dto.Situacao == SituacaoEnum.Aberto)
                return true;

            NotificarValidacaoDominio("Não é possivel abrir o caixa com o estado fechado");

            return false;
        }

       private async Task AlterarCaixaExistenteAsync(FluxoDeCaixaSituacaoDto dto)
        {
            if (dto.Id == 0)
                return;

            var fluxoDeCaixa = await _fluxoDeCaixaRepository.ObterPorIdAsync(dto.Id);

            if (fluxoDeCaixa == null)
            {
                NotificarValidacaoDominio("Caixa não encontrado");
                return;
            }
                

            fluxoDeCaixa.AlterarSituacao(dto.Situacao);
        }
    }
}
