using FDC.Generics.Api.Controllers;
using FDC.Generics.Domain;
using FDC.Seguranca.Api.Domain.Identity.Interfaces;
using FDC.Seguranca.Api.Domain.Token.Interfaces;
using FDC.Seguranca.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FDC.Seguranca.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly ISignInManagerService _signInManagerService;
        private readonly IUserManagerService _userManagerService;

        public AuthController(
            ITokenService tokenService, 
            ISignInManagerService signInManagerService, 
            IUserManagerService userManagerService,
            IDomainNotificationService<DomainNotification> notificacaoDeDominio)
            : base(notificacaoDeDominio)
        {
            _tokenService = tokenService;
            _signInManagerService = signInManagerService;
            _userManagerService = userManagerService;
        }

        [HttpPost("criar-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _userManagerService.CreateAsync(usuarioRegistro);

            if (result.Succeeded)
            {
                return CustomResponse(await _tokenService.GerarJwt(usuarioRegistro.Email));
            }

            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("entrar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManagerService.PasswordSignInAsync(usuarioLogin);

            if (result.Succeeded)
            {
                return CustomResponse(await _tokenService.GerarJwt(usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha incorretos");
            return CustomResponse();
        }  
    }
}
