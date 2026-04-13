using ProductService.Core.DTOs; 

namespace ProductService.Core.Interfaces.Services
{

    public interface IProductService
    {
        ProductDto GetProductById(int id);
    }

}
