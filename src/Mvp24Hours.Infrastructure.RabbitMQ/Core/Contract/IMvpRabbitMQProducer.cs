//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

namespace Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract
{
    public interface IMvpRabbitMQProducer
    {
        public void Publish(object message, string routingKey = null, string tokenDefault = null);
    }
}
