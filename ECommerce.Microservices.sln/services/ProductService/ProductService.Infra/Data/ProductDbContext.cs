using Microsoft.EntityFrameworkCore;
using ProductService.Core.Entities;

namespace ProductService.Infra.Data
{
    public  class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
