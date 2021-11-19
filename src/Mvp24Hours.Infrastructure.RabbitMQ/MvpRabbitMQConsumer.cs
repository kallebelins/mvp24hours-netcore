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
            CreateEvent();
        }

        protected MvpRabbitMQConsumer(string routingKey)
            : base(routingKey)
        {
            CreateEvent();
        }

        protected MvpRabbitMQConsumer(string hostAddress, string routingKey)
            : base(hostAddress, routingKey)
        {
            CreateEvent();
        }

        protected MvpRabbitMQConsumer(RabbitMQConfiguration configuration)
            : base(configuration)
        {
            CreateEvent();
        }

        protected MvpRabbitMQConsumer(RabbitMQConfiguration configuration, RabbitMQQueueOptions options)
            : base(configuration, options)
        {
            CreateEvent();
        }

        private void CreateEvent()
        {
            if (_event == null)
            {
                _event = new EventingBasicConsumer(Channel);
                _event.Received += Event_Received;
                Channel.BasicConsume(queue: Options.RoutingKey,
                     autoAck: false,
                     consumer: _event);
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
            catch (Exception)
            {
                Channel.BasicNack(e.DeliveryTag, false, true);
            }
        }

        public abstract Task Received(T message);
    }
}
