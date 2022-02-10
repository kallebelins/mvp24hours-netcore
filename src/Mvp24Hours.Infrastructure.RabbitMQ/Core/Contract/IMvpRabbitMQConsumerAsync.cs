//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQConsumerAsync
    {
        void Consume(string queueName = null, string routingKey = null);
        Task ReceivedAsync(object message);
    }

    public interface IMvpRabbitMQConsumerAsync<T> : IMvpRabbitMQConsumerAsync
        where T : class
    {
    }
}
