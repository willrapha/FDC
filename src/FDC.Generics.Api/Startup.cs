using Autofac;
using FDC.Generics.Bus;
using FDC.Generics.Bus.Abstractations;
using FDC.Generics.Bus.InMemory;
using FDC.Generics.Bus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FDC.Generics.Api
{
    public static class Startup
    {
        public static void AddRabbitMQConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //EventBus
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddSingleton<IEventBus, EventBus>(sp =>
            {
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var rabbitMQConnection = sp.GetRequiredService<IRabbitMQConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBus>>();
                var lifetimeScope = sp.GetRequiredService<ILifetimeScope>();

                return new EventBus(eventBusSubcriptionsManager, rabbitMQConnection, logger, lifetimeScope);
            });

            //RabbitMQ
            services.AddSingleton<IRabbitMQConnection>(sp =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMQ:HostName"],
                    Port = int.Parse(configuration["RabbitMQ:Port"]),
                    UserName = configuration["RabbitMQ:UserName"],
                    Password = configuration["RabbitMQ:Password"]
                };

                return new RabbitMQConnection(connectionFactory);
            });
        }
    }
}
