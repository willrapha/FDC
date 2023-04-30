using FDC.Generics.Domain;
using FDC.Seguranca.Api.Domain.Identity.Interfaces;
using FDC.Seguranca.Api.Domain.Identity.Services;
using FDC.Seguranca.Api.Domain.Token.Interfaces;
using FDC.Seguranca.Api.Domain.Token.Services;

namespace FDC.Seguranca.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISignInManagerService, SignInManagerService>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IDomainNotificationService<DomainNotification>, DomainNotificationService>();
        }
    }
}
