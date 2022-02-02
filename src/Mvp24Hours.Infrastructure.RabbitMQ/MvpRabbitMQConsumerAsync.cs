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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQConsumerAsync<T> : MvpRabbitMQBase, IMvpRabbitMQConsumerAsync<T>
        where T : class
    {
        protected override bool DispatchConsumersAsync => true;
        protected event EventHandler<Exception, BasicDeliverEventArgs> Failure;

        private AsyncEventingBasicConsumer _event;

        protected MvpRabbitMQConsumerAsync()
            : base(typeof(T).Name)
        {
        }

        protected MvpRabbitMQConsumerAsync(string routingKey)
            : base(routingKey)
        {
        }

        protected MvpRabbitMQConsumerAsync(string hostAddress, string routingKey)
            : base(hostAddress, routingKey)
        {
        }

        protected MvpRabbitMQConsumerAsync(RabbitMQConfiguration configuration, string routingKey)
            : base(configuration, routingKey)
        {
        }

        protected MvpRabbitMQConsumerAsync(RabbitMQConfiguration configuration, RabbitMQQueueOptions options)
            : base(configuration, options)
        {
        }

        public virtual void Consume()
        {
            try
            {
                if (_event == null)
                {
                    _event = new AsyncEventingBasicConsumer(Channel);
                    _event.Received += EventReceivedAsync;

                    Channel.QueueBind(queue: Options.Queue ?? string.Empty,
                                            exchange: Options.Exchange,
                                            routingKey: Options.RoutingKey);
                }

                Channel.BasicConsume(queue: Options.Queue ?? string.Empty,
                     autoAck: Options.AutoAck,
                     consumer: _event);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        private async Task EventReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                string messageString = Encoding.UTF8.GetString(body);
                T message = messageString.ToDeserialize<T>();
                await ReceivedAsync(message);
                if (!Options.AutoAck)
                {
                    Channel.BasicAck(e.DeliveryTag, false);
                }
            }
            catch (Exception ex)
            {
                if (!Options.AutoAck)
                {
                    Channel.BasicNack(e.DeliveryTag, false, true);
                }
                Logging.Error(ex);
                Failure?.Invoke(ex, e);
            }
        }

        public abstract Task ReceivedAsync(T message);
    }
}
