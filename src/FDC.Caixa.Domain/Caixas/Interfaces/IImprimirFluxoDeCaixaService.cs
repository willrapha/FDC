using FDC.Caixa.Domain.Caixas.Dtos;
using FDC.Generics.Domain;

namespace FDC.Caixa.Domain.Caixas.Interfaces
{
    public interface IImprimirFluxoDeCaixaService
    {
        Task<ArquivoDto> Imprimir(long id, string token);
    }
}
