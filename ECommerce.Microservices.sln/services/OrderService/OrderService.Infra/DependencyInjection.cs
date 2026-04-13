using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Services;
using OrderService.Infra.HttpClients;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient<ProductHttpClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5035"); // ProductService URL
            });

            services.AddScoped<IOrderService, OrderService.Infra.Services.OrderService>();

            return services;
        }
    }
}
