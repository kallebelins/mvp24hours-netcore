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
    public abstract class MvpRabbitMQConsumer<T> : MvpRabbitMQConsumer, IMvpRabbitMQConsumer<T>
        where T : class
    {
        #region [ Ctors ]
        protected MvpRabbitMQConsumer(IOptions<RabbitMQOptions> options)
            : base(options, queueName: typeof(T).Name)
        {
        }
        #endregion

        #region [ Methods ]
        public abstract void Received(T message);
        #endregion
    }
}
