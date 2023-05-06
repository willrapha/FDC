using FDC.Caixa.Domain.Caixas.Interfaces;
using FDC.Caixa.Domain.Caixas.Services;
using FDC.Caixa.Infra.Data.Context;
using FDC.Caixa.Infra.Data.Repositories;
using FDC.Caixa.Infra.Data.Rest.Caixas;
using FDC.Caixa.Infra.IoC.AutoMapper;
using FDC.Generics.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FDC.Caixa.Infra.IoC
{
    public static class Startup
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {          
            services.AddScoped<IDomainNotificationService<DomainNotification>, DomainNotificationService>();

            services.AddScoped<IAlterarSituacaoFluxoDeCaixaService, AlterarSituacaoFluxoDeCaixaService>();
            services.AddScoped<IArmazenadorDeMovimentacaoService, ArmazenadorDeMovimentacaoService>();
            services.AddScoped<IObterFluxoDeCaixaService, ObterFluxoDeCaixaService>();
            services.AddScoped<IImprimirFluxoDeCaixaService, ImprimirFluxoDeCaixaService>();

            services.AddScoped<IImprimirFluxoDeCaixaRest, ImprimirFluxoDeCaixaRest>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
            services.AddScoped<IFluxoDeCaixaRepository, FluxoDeCaixaRepository>();

            services.AddSingleton(AutoMapperConfiguration.Initialize().CreateMapper().RegisterMap());
        }
    }
}
