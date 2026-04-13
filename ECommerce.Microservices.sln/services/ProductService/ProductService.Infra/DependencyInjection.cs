using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Core.Interfaces.Repositories;
using ProductService.Core.Interfaces.Services;
using ProductService.Infra.Data;
using ProductService.Infra.Repositories;

namespace ProductService.Infra
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<ProductDbContext>(options =>
                options.UseInMemoryDatabase("ProductDb"));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, Services.ProductService>();

            return services;
        }
    }
}
