using FDC.Caixa.Domain.Caixas.Entities;
using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Caixa.Domain.Caixas.Services;
using FDC.Caixa.Infra.Data.Context;
using FDC.Generics.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FDC.Caixa.Infra.IoC
{
    public static class Startup
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FluxoDeCaixaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDomainNotificationService<DomainNotification>, DomainNotificationService>();
            services.AddScoped<IArmazenadorDeFluxoDeCaixaService, ArmazenadorDeFluxoDeCaixaService>();
        }
    }
}
