using Microsoft.Extensions.DependencyInjection;


namespace OrderService.Core
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            return services;
        }
    }

}
