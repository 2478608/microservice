using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Services;
using OrderService.Infra.HttpClients;
using OrderService.Infra.Messaging.Kafka;
using OrderService.Infra.Messaging.RabbitMQ;
using OrderService.Infra.Policies;

namespace OrderService.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient<ProductHttpClient>(client =>
            {
                // ✅ Docker service name (NOT localhost)
                client.BaseAddress = new Uri("http://product-service:5035/");
            })
            .AddPolicyHandler(PollyPolicies.CombinedPolicy);

            services.AddScoped<IOrderService, Services.OrderService>();

            services.AddSingleton<OrderEventPublisher>();
            services.AddSingleton<KafkaOrderProducer>();

            return services;
        }
    }
}