using FDC.Caixa.Api.Configuration;
using FDC.Caixa.Infra.Data;
using FDC.Caixa.Infra.Data.Context;
using FDC.Caixa.Infra.IoC;
using FDC.Generics.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfiguration(builder.Configuration);
app.UseApiConfiguration(builder.Environment);
app.Services.RunMigration<FluxoDeCaixaContext>();

app.Run();
