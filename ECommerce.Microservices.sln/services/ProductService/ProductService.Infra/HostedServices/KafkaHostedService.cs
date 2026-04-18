using Microsoft.Extensions.Hosting;
using ProductService.Infra.Messaging.Kafka;

namespace ProductService.Infra.HostedServices
{
    public class KafkaHostedService : BackgroundService
    {
        private readonly KafkaOrderConsumer _consumer;

        public KafkaHostedService(KafkaOrderConsumer consumer)
        {
            _consumer = consumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Start();
            return Task.CompletedTask;
        }
    }
}