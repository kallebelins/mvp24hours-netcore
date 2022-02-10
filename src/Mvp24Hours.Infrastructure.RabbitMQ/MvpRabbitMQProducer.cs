//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQProducer : MvpRabbitMQBase, IMvpRabbitMQProducer
    {
        #region [ Ctors ]
        protected MvpRabbitMQProducer(IOptions<RabbitMQOptions> options, string queueName = null, string routingKey = null)
           : base(options?.Value, queueName, routingKey: routingKey)
        {
        }
        protected MvpRabbitMQProducer(RabbitMQOptions options, string queueName = null, string routingKey = null)
           : base(options, queueName, routingKey: routingKey)
        {
        }
        #endregion

        #region [ Methods ]
        public virtual void Publish(object message, string routingKey = null, string tokenDefault = null)
        {
            try
            {
                var bsEvent = message.ToBusinessEvent(tokenDefault: tokenDefault);
                var body = Encoding.UTF8.GetBytes(bsEvent.ToSerialize(JsonHelper.JsonBusinessEventSettings()));

                Channel.BasicPublish(exchange: Options.Exchange,
                                     routingKey: routingKey ?? Options.RoutingKey,
                                     basicProperties: Options.BasicProperties,
                                     body: body);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }
        #endregion
    }
}
