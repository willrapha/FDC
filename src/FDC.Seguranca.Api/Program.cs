using Autofac.Extensions.DependencyInjection;
using FDC.Generics.Api;
using FDC.Seguranca.Api.Configuration;
using FDC.Seguranca.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddApiConfiguration();
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.RegisterServices();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddRabbitMQConfiguration(builder.Configuration);


var app = builder.Build();

app.UseSwaggerConfiguration(builder.Configuration);
app.UseApiConfiguration(builder.Environment);
app.Services.RunMigration<ApplicationDbContext>();

app.Run();
