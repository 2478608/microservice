using Confluent.Kafka;

namespace ProductService.Infra.Messaging.Kafka
{
    public class KafkaOrderConsumer
    {
        public void Start()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:9092",
                GroupId = "product-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("order-events");

            while (true)
            {
                var msg = consumer.Consume();
                Console.WriteLine($"📥 Kafka event: {msg.Message.Value}");
            }
        }
    }
}
