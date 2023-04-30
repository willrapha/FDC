using AutoMapper;
using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Caixa.Domain.Caixas.Interfaces;

namespace FDC.Caixa.Domain.Caixas.Services
{
    public class ObterFluxoDeCaixaService : IObterFluxoDeCaixaService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IMapper _mapper;

        public ObterFluxoDeCaixaService(IFluxoDeCaixaRepository fluxoDeCaixaRepository, IMapper mapper)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
            _mapper = mapper;
        }

        public async Task<FluxoDeCaixaDto> Obter(long id)
        {
            var fluxo = await _fluxoDeCaixaRepository.ObterFluxoComMovimentacao(id);

            return _mapper.Map<FluxoDeCaixaDto>(fluxo);
        }
    }
}
