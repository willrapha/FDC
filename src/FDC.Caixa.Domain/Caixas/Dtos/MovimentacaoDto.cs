using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Enums;

namespace FDC.Caixa.Domain.Caixas.Dtos
{
    public class MovimentacaoDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public TipoEmum Tipo { get; set; }
        public long FluxoDeCaixaId { get; set; }
    }
}
