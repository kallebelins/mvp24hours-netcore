//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQConsumer : MvpRabbitMQBase, IMvpRabbitMQConsumer
    {
        #region [ Properties / Fields ]
        protected event EventHandler<Exception, BasicDeliverEventArgs> Failure;
        private EventingBasicConsumer _event;
        #endregion

        #region [ Ctors ]
        protected MvpRabbitMQConsumer(IOptions<RabbitMQOptions> options, string queueName = null, string routingKey = null)
           : base(options?.Value, queueName, routingKey: routingKey)
        {
        }
        protected MvpRabbitMQConsumer(RabbitMQOptions options, string queueName = null, string routingKey = null)
           : base(options, queueName, routingKey: routingKey)
        {
        }
        #endregion

        #region [ Methods ]
        public virtual void Consume(string queueName = null, string routingKey = null)
        {
            try
            {
                if (_event == null)
                {
                    _event = new EventingBasicConsumer(Channel);
                    _event.Received += EventReceived;

                    Channel.QueueBind(queue: queueName ?? Options.Queue ?? string.Empty,
                                            exchange: Options.Exchange,
                                            routingKey: routingKey ?? Options.RoutingKey);
                }

                Channel.BasicConsume(queue: queueName ?? Options.Queue ?? string.Empty,
                     autoAck: Options.AutoAck,
                     consumer: _event);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        protected virtual void EventReceived(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                string messageString = Encoding.UTF8.GetString(body);
                IBusinessEvent message = messageString.ToDeserialize<IBusinessEvent>(JsonHelper.JsonBusinessEventSettings());
                Received(message.GetDataObject());
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

        public abstract void Received(object message);
        #endregion
    }
}
