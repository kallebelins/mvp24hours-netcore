//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQProducer<T> : MvpRabbitMQProducer, IMvpRabbitMQProducer
        where T : class
    {
        #region [ Ctors ]
        protected MvpRabbitMQProducer(IOptions<RabbitMQOptions> options)
            : base(options?.Value, queueName: typeof(T).Name, routingKey: typeof(T).Name)
        {
        }
        #endregion
    }
}
