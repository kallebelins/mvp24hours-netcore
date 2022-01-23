//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.ValueObjects.RabbitMQ;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Enums;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQProducer<T> : MvpRabbitMQBase, IMvpRabbitMQProducer<T>
        where T : class
    {
        protected MvpRabbitMQProducer()
            : base(typeof(T).Name)
        {
        }

        protected MvpRabbitMQProducer(string routingKey)
            : base(routingKey)
        {
        }

        protected MvpRabbitMQProducer(string hostAddress, string routingKey)
            : base(hostAddress, routingKey)
        {
        }

        protected MvpRabbitMQProducer(RabbitMQConfiguration configuration)
            : base(configuration)
        {
        }

        protected MvpRabbitMQProducer(RabbitMQConfiguration configuration, RabbitMQQueueOptions options)
            : base(configuration, options)
        {
        }

        public virtual void Publish(T message, MvpRabbitMQPriorityEnum priorityEnum = MvpRabbitMQPriorityEnum.Normal)
        {
            Publish(message.ToSerialize(), priorityEnum);
        }

        public virtual void Publish(string message, MvpRabbitMQPriorityEnum priorityEnum = MvpRabbitMQPriorityEnum.Normal)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                Channel.BasicPublish(exchange: Options.Exchange,
                                     routingKey: Options.OverwiteRoutingKey ?? priorityEnum.ToString(),
                                     basicProperties: Options.BasicProperties,
                                     body: body);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

    }
}
