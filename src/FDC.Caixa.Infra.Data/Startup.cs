using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FDC.Caixa.Infra.Data
{
    public static class Startup
    {
        public static void RunMigration<T>(this IServiceProvider app) where T : DbContext
        {
            using (var serviceScope = app.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<T>();
                context.Database.SetCommandTimeout(3 * 1000);
                context.Database.Migrate();
            }
        }
    }
}
