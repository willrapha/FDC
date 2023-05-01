using AutoMapper;
using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Services
{
    public class ImprimirFluxoDeCaixaService : DomainService, IImprimirFluxoDeCaixaService
    {
        private readonly IImprimirFluxoDeCaixaRest _imprimirFluxoDeCaixaRest;
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IMapper _mapper;

        public ImprimirFluxoDeCaixaService(
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IImprimirFluxoDeCaixaRest imprimirFluxoDeCaixaRest,
            IFluxoDeCaixaRepository fluxoDeCaixaRepository,
            IMapper mapper)
            : base(notificacaoDeDominio)
        {
            _imprimirFluxoDeCaixaRest = imprimirFluxoDeCaixaRest;
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
            _mapper = mapper;
        }

        public async Task<ArquivoDto> Imprimir(long id, string token)
        {
            var fluxo = await _fluxoDeCaixaRepository.ObterFluxoComMovimentacao(id);

            if (fluxo == null)
                NotificarValidacaoDominio("Fluxo de caixa não encontrado");

            var dto = _mapper.Map<FluxoDeCaixaImprimirDto>(fluxo);
            var arquivo = await _imprimirFluxoDeCaixaRest.ObterPorId(dto, token);

            if (arquivo == null)
                NotificarValidacaoDominio("Arquivo não encontrado");

            return arquivo;
        }

    }
}
