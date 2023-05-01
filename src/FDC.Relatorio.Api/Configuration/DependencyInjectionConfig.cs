using FDC.Generics.Domain;
using FDC.Relatorio.Domain.FluxoDeCaixa.Interfaces;
using FDC.Relatorio.Domain.FluxoDeCaixa.Services;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Interfaces;
using FDC.Relatorio.Domain.GeradorDeRelatorio.Services;

namespace FDC.Relatorio.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IGeradorDeFluxoDeCaixaRelatorioService, GeradorDeFluxoDeCaixaRelatorioService>();
            services.AddScoped<IGeradorDeRelatorioService, GeradorDeRelatorioService>();
            services.AddScoped<IDomainNotificationService<DomainNotification>, DomainNotificationService>();
        }
    }
}
