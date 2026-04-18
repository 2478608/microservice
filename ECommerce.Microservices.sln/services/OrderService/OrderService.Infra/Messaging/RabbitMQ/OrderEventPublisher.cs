using RabbitMQ.Client;
using Shared.Events;
using System.Text;
using System.Text.Json;

namespace OrderService.Infra.Messaging.RabbitMQ
{
    public class OrderEventPublisher
    {
        public void Publish(OrderCreatedEvent evt)
        {
            using var connection = RabbitMqConnection.Create();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: "order.exchange",
                type: ExchangeType.Topic,
                durable: true
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));

            channel.BasicPublish(
                exchange: "order.exchange",
                routingKey: "order.created",
                basicProperties: null,
                body: body
            );
        }
    }
}
