using ProductService.Core.Entities; 

namespace ProductService.Core.Interfaces.Repositories
{

    public interface IProductRepository
    {
        Product? GetById(int id);
    }

}
