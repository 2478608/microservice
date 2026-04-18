using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Events;
using System.Text;
using System.Text.Json;

namespace ProductService.Infra.Messaging.RabbitMQ
{
    public class OrderCreatedConsumer
    {
        public void Start()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "admin",
                Password = "admin"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("order.exchange", ExchangeType.Topic, true);
            channel.QueueDeclare("order.created.queue", true, false, false);
            channel.QueueBind("order.created.queue", "order.exchange", "order.created");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (s, e) =>
            {
                var json = Encoding.UTF8.GetString(e.Body.ToArray());
                var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                Console.WriteLine($"✅ RabbitMQ received Order {evt!.ProductId}");
            };

            channel.BasicConsume("order.created.queue", true, consumer);
        }
    }
}