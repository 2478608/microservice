using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Interfaces.Services;
using OrderService.Infra.HttpClients;
using OrderService.Infra.Policies;

namespace OrderService.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpClient<ProductHttpClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5035");
            })
            .AddPolicyHandler(PollyPolicies.CombinedPolicy);

            services.AddScoped<IOrderService, Services.OrderService>();

            return services;
        }
    }
}
