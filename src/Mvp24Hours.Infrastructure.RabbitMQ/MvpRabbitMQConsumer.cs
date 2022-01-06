//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.ValueObjects.RabbitMQ;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQConsumer<T> : MvpRabbitMQBase, IMvpRabbitMQConsumer<T>
        where T : class
    {
        private EventingBasicConsumer _event;

        protected MvpRabbitMQConsumer()
            : base()
        {
        }

        protected MvpRabbitMQConsumer(string routingKey)
            : base(routingKey)
        {
        }

        protected MvpRabbitMQConsumer(string hostAddress, string routingKey)
            : base(hostAddress, routingKey)
        {
        }

        protected MvpRabbitMQConsumer(RabbitMQConfiguration configuration)
            : base(configuration)
        {
        }

        protected MvpRabbitMQConsumer(RabbitMQConfiguration configuration, RabbitMQQueueOptions options)
            : base(configuration, options)
        {
        }

        public virtual void Consume()
        {
            try
            {
                if (_event == null)
                {
                    _event = new EventingBasicConsumer(Channel);
                    _event.Received += Event_Received;
                }
                Channel.BasicConsume(queue: Options.RoutingKey,
                     autoAck: false,
                     consumer: _event);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        private void Event_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);
                var message = messageString.ToDeserialize<T>();
                Task.Run(async () =>
                {
                    await Received(message);
                    Channel.BasicAck(e.DeliveryTag, false);
                });
            }
            catch (Exception ex)
            {
                Channel.BasicNack(e.DeliveryTag, false, true);
                Logging.Error(ex);
                throw;
            }
        }

        public abstract Task Received(T message);
    }
}
