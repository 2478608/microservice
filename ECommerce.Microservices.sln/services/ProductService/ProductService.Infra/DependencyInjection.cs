using Microsoft.Extensions.DependencyInjection;
using ProductService.Infra.Messaging.Kafka;
using ProductService.Infra.Messaging.RabbitMQ;
using ProductService.Infra.HostedServices;

namespace ProductService.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddSingleton<OrderCreatedConsumer>();
            services.AddSingleton<KafkaOrderConsumer>();

            services.AddHostedService<RabbitMqHostedService>();
            services.AddHostedService<KafkaHostedService>();

            return services;
        }
    }
}