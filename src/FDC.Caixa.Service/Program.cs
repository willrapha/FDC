using Autofac.Extensions.DependencyInjection;
using FDC.Caixa.Service.Events;
using FDC.Caixa.Service.Handlers;
using FDC.Generics.Api;
using FDC.Generics.Bus.Abstractations;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.Diagnostics;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateBuilder(options);
builder.Host.UseWindowsService();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllers();
builder.Services.AddSingleton(provider => builder.Configuration);
builder.Services.AddRabbitMQConfiguration(builder.Configuration);
builder.Services.AddTransient<PessoaFisicaEventHandler>();

var app = builder.Build();

app.MapGet("/ping", () =>
{
    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
    return fvi.FileVersion;
});

var eventBus = app.Services.GetRequiredService<IEventBus>();

eventBus.Subscribe<PessoaFisicaEvent, PessoaFisicaEventHandler>(
    "fdc-integracao-pessoa-fisica",
    "fdc-integracao-pessoa-fisica",
    prefetchCount: 10,
    deadLetter: true);

app.Run();