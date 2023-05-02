using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FDC.Seguranca.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FDC Segurança API",
                    Description = "Esta API é responsavel pela autenticação e autorização do usuario",
                    Contact = new OpenApiContact() { Name = "Willian Raphael", Email = "willian.mattos@gmail.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            var basePath = configuration["AppSettings:BasePath"];

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{basePath}" } };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "API V1");
                c.InjectStylesheet($"{basePath}/swagger-ui/custom.css");
                c.DocExpansion(DocExpansion.None);
                c.OAuthClientId("swagger-ui");
                c.OAuthClientSecret("swagger-ui-secret");
                c.OAuthRealm("swagger-ui-realm");
                c.OAuthAppName("Swagger UI");
            });

            return app;
        }
    }
}
