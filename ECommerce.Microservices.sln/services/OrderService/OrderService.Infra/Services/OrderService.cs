using OrderService.Core.DTOs;
using OrderService.Core.Interfaces.Services;
using OrderService.Infra.HttpClients;
using OrderService.Infra.Messaging.Kafka;
using OrderService.Infra.Messaging.RabbitMQ;
using Shared.Events;

namespace OrderService.Infra.Services
{
    public class OrderService : IOrderService
    {
        private readonly ProductHttpClient _productClient;
        private readonly OrderEventPublisher _rabbitPublisher;
        private readonly KafkaOrderProducer _kafkaProducer;

        public OrderService(
            ProductHttpClient productClient,
            OrderEventPublisher rabbitPublisher,
            KafkaOrderProducer kafkaProducer)
        {
            _productClient = productClient;
            _rabbitPublisher = rabbitPublisher;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<OrderResponseDto> PlaceOrderAsync(CreateOrderRequestDto request)
        {
            var product = await _productClient.GetProductAsync(request.ProductId);

            var total = product.Price * request.Quantity;

            var evt = new OrderCreatedEvent(
                request.ProductId,
                request.Quantity,
                total
            );

            _rabbitPublisher.Publish(evt);
            await _kafkaProducer.PublishAsync(evt);

            return new OrderResponseDto
            {
                ProductId = request.ProductId,
                TotalAmount = total
            };
        }
    }
}