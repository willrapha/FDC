using FDC.Seguranca.Api.Models;

namespace FDC.Seguranca.Api.Domain.Token.Interfaces
{
    public interface ITokenService
    {
        Task<UsuarioRespostaLogin> GerarJwt(string email);
    }
}
