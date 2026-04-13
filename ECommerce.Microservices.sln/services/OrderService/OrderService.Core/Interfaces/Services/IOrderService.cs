using OrderService.Core.DTOs;

namespace OrderService.Core.Interfaces.Services
{

    public interface IOrderService
    {
        Task<OrderResponseDto> PlaceOrderAsync(CreateOrderRequestDto request);
    }

}
