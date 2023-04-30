using FDC.Seguranca.Api.Domain.Identity.Interfaces;
using FDC.Seguranca.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FDC.Seguranca.Api.Domain.Identity.Services
{
    public class SignInManagerService: ISignInManagerService
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public SignInManagerService(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> PasswordSignInAsync(UsuarioLogin usuarioLogin)
        {
            return await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);
        }
    }
}
