using Microsoft.AspNetCore.Mvc;
using OrderService.Core.DTOs;
using OrderService.Core.Interfaces.Services;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CreateOrderRequestDto request)
        {
            var result = await _service.PlaceOrderAsync(request);
            return Ok(result);
        }
    }
}