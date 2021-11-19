using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.RabbitMQ.ValueObjects;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class BaseMvpProducer
    {
        private readonly RabbitMQConfiguration _brokerConfiguration;
        private readonly ConnectionFactory _factory;

        protected RabbitMQConfiguration Configuration => _brokerConfiguration;
        protected ConnectionFactory Factory => _factory;

        public BaseMvpProducer(string routingKey)
            : this(string.Empty, routingKey)
        {
        }

        public BaseMvpProducer(string exchange, string routingKey)
            : this("Mvp24Hours:Brokers:RabbitMQ", exchange, routingKey)
        {
        }


        public BaseMvpProducer(string configurationSection, string exchange, string routingKey)
        {
            _brokerConfiguration = ConfigurationHelper.GetSettings<RabbitMQConfiguration>(configurationSection);

            _factory = new ConnectionFactory()
            {
                HostName = _brokerConfiguration.HostName,
                Port = _brokerConfiguration.Port,
                UserName = _brokerConfiguration.UserName,
                Password = _brokerConfiguration.Password
            };
        }

        private void Publish(string routingKey, string message)
        {
            

            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: routingKey,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
