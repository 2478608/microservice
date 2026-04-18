using Microsoft.Extensions.Hosting;
using ProductService.Infra.Messaging.RabbitMQ;

namespace ProductService.Infra.HostedServices
{
    public class RabbitMqHostedService : IHostedService
    {
        private readonly OrderCreatedConsumer _consumer;

        public RabbitMqHostedService(OrderCreatedConsumer consumer)
        {
            _consumer = consumer;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _consumer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    }
}