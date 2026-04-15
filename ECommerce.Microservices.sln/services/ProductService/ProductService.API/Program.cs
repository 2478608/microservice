using ProductService.Core;
using ProductService.Core.Interfaces.Services;
using ProductService.Infra;
using ProductService.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ✅ Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    var baseConn = builder.Configuration["Redis:ConnectionString"];
    var password = builder.Configuration["Redis:Password"];

    if (string.IsNullOrWhiteSpace(password))
        throw new InvalidOperationException(
            "Redis password is missing. Set Redis__Password environment variable.");

    options.Configuration = $"{baseConn},password={password}";
});

builder.Services.AddCore();
builder.Services.AddInfrastructure("");

var app = builder.Build();

// ✅ Seed + invalidate cache (FIXED)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    var service = scope.ServiceProvider.GetRequiredService<IProductService>();

    db.Products.Add(new ProductService.Core.Entities.Product { Id = 1, Name = "Laptop", Price = 50000 });
    db.Products.Add(new ProductService.Core.Entities.Product { Id = 2, Name = "Phone", Price = 30000 });
    db.SaveChanges();

    // ✅ safe cast to access invalidation
    if (service is ProductService.Infra.Services.ProductService concrete)
    {
        concrete.InvalidateProductCache(1);
        concrete.InvalidateProductCache(2);
    }
}

app.MapControllers();
app.Run();
