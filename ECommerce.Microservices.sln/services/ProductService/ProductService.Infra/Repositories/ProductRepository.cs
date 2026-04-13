using ProductService.Core.Entities;
using ProductService.Core.Interfaces.Repositories;
using ProductService.Infra.Data;

namespace ProductService.Infra.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public Product? GetById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
