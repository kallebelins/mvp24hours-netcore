//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQConsumerAsync<T> : MvpRabbitMQConsumerAsync, IMvpRabbitMQConsumerAsync<T>
        where T : class
    {
        #region [ Ctors ]
        protected MvpRabbitMQConsumerAsync(IOptions<RabbitMQOptions> options)
            : base(options?.Value, queueName: typeof(T).Name)
        {
        }
        #endregion

        #region [ Methods ]
        public abstract Task ReceivedAsync(T message);
        #endregion
    }
}
