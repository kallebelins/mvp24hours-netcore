//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQProducer<T> : MvpRabbitMQProducer
        where T : class
    {
        #region [ Ctors ]
        protected MvpRabbitMQProducer(IOptions<RabbitMQOptions> options, ILoggingService logging)
            : base(options?.Value, logging, queueName: typeof(T).Name, routingKey: typeof(T).Name)
        {
        }
        #endregion
    }
}
