//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQConsumer
    {
        void Consume(string queueName = null, string routingKey = null);
        void Received(object message);
    }

    public interface IMvpRabbitMQConsumer<T> : IMvpRabbitMQConsumer
        where T : class
    {
    }
}
