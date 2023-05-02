using FDC.Generics.Api.Controllers;
using FDC.Generics.Bus;
using FDC.Generics.Bus.Abstractations;
using FDC.Generics.Domain;
using FDC.Seguranca.Api.Domain.Identity.Interfaces;
using FDC.Seguranca.Api.Domain.Token.Interfaces;
using FDC.Seguranca.Api.Events;
using FDC.Seguranca.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FDC.Seguranca.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly ISignInManagerService _signInManagerService;
        private readonly IUserManagerService _userManagerService;
        private readonly IEventBus _eventBus;

        public AuthController(
            ITokenService tokenService,
            ISignInManagerService signInManagerService,
            IUserManagerService userManagerService,
            IDomainNotificationService<DomainNotification> notificacaoDeDominio,
            IEventBus eventBus)
            : base(notificacaoDeDominio)
        {
            _tokenService = tokenService;
            _signInManagerService = signInManagerService;
            _userManagerService = userManagerService;
            _eventBus = eventBus;
        }

        [HttpPost("criar-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _userManagerService.CreateAsync(usuarioRegistro);

            if (result.Succeeded)
            {
                var evento = EventBusOptions.Config(
                "fdc-integracao-pessoa-fisica",
                "fdc-integracao-pessoa-fisica",
                withDeadletter: true);

                var usuario = new UsuarioEvent()
                {
                    Email = usuarioRegistro.Email,
                };

                _eventBus.Publish(usuario, evento);

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
