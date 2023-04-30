using FDC.Seguranca.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FDC.Seguranca.Api.Domain.Identity.Interfaces
{
    public interface ISignInManagerService
    {
        Task<SignInResult> PasswordSignInAsync(UsuarioLogin usuarioLogin);
    }
}
