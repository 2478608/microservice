using Confluent.Kafka;
using Shared.Events;
using System.Text.Json;

namespace OrderService.Infra.Messaging.Kafka
{
    public class KafkaOrderProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaOrderProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:9092"
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync(OrderCreatedEvent evt)
        {
            await _producer.ProduceAsync(
                KafkaTopics.OrderEvents,
                new Message<string, string>
                {
                    Key = evt.ProductId.ToString(),
                    Value = JsonSerializer.Serialize(evt)
                });
        }
    }
}