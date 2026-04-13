
using ProductService.Core;
using ProductService.Core.Entities;
using ProductService.Infra;
using ProductService.Infra.Data;

namespace ProductService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddCore();
            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default") ?? "");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }


            // Seed data
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                db.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 50000 });
                db.Products.Add(new Product { Id = 2, Name = "Phone", Price = 30000 });
                db.SaveChanges();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
