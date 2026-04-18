using RabbitMQ.Client;

namespace OrderService.Infra.Messaging.RabbitMQ
{
    public static class RabbitMqConnection
    {
        public static IConnection Create()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "admin",
                Password = "admin"
            };

            return factory.CreateConnection();
        }
    }
}