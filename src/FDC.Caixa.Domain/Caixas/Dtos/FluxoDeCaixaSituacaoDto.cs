using FDC.Caixa.Domain.Caixas.Enums;

namespace FDC.Caixa.Domain.Caixas.Dtos
{
    public class FluxoDeCaixaSituacaoDto
    {
        public long Id { get; set; }
        public SituacaoEnum Situacao { get; set; }
    }
}
