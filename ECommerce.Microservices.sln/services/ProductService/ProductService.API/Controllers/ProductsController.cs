using Microsoft.AspNetCore.Mvc;
using ProductService.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var prod = _productService.GetProductById(id);
            return Ok(prod);
        }
    }
}
