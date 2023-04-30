using FDC.Caixa.Domain.Caixas.Dtos;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IArmazenadorDeMovimentacaoService
    {
        Task Armazenar(MovimentacaoDto dto);
    }
}
