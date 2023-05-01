using Microsoft.EntityFrameworkCore;

namespace FDC.Seguranca.Api.Configuration
{
    public static class MigrationConfig
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
