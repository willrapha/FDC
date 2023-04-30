using FDC.Seguranca.Api.Domain.Identity.Interfaces;
using FDC.Seguranca.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FDC.Seguranca.Api.Domain.Identity.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagerService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(UsuarioRegistro usuarioRegistro)
        {
            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            return await _userManager.CreateAsync(user, usuarioRegistro.Senha);
        }


    }
}
