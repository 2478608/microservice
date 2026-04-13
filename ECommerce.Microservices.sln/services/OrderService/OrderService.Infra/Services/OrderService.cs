using OrderService.Core.DTOs;
using OrderService.Core.Interfaces.Services;
using OrderService.Infra.HttpClients;

namespace OrderService.Infra.Services
{
    public class OrderService : IOrderService
    {
        private readonly ProductHttpClient _productClient;

        public OrderService(ProductHttpClient productClient)
        {
            _productClient = productClient;
        }
        public async Task<OrderResponseDto> PlaceOrderAsync(CreateOrderRequestDto request)
        {
            var prod = await _productClient.GetProductAsync(request.ProductId);
            return new OrderResponseDto
            {
                ProductId = request.ProductId,
                TotalAmount = prod.Price * request.Quantity
            };
        }
    }
}
