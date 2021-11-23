//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.ValueObjects.RabbitMQ;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Logging;
using RabbitMQ.Client;
using System;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQBase : IDisposable
    {
        private readonly RabbitMQConfiguration _brokerConfiguration;
        private readonly RabbitMQQueueOptions _queueOptions;
        private readonly ConnectionFactory _factory;
        private ILoggingService _logging;

        private IModel _channel;
        private IConnection _connection;

        protected RabbitMQConfiguration Configuration => _brokerConfiguration;
        protected RabbitMQQueueOptions Options => _queueOptions;
        protected IConnection Connection => _connection;
        protected IModel Channel
        {
            get
            {
                if (_channel == null || _channel.IsClosed)
                {
                    _channel = CreateChannel();
                }
                return _channel;
            }
        }

        protected ILoggingService Logging => _logging ??= LoggingService.GetLoggingService();

        protected MvpRabbitMQBase()
            : this("mvp24hours-queue")
        {
        }

        protected MvpRabbitMQBase(string routingKey)
            : this("Mvp24Hours:Brokers:RabbitMQ", routingKey)
        {
        }

        protected MvpRabbitMQBase(string hostAddress, string routingKey)
            : this(
                  ConfigurationHelper.GetSettings<RabbitMQConfiguration>(hostAddress)
                        ?? throw new ArgumentException("Host address not found in settings."),
                  new RabbitMQQueueOptions
                  {
                      RoutingKey = routingKey
                      ?? throw new ArgumentException("Route key is mandatory.")
                  })
        {
        }

        protected MvpRabbitMQBase(RabbitMQConfiguration configuration)
            : this(configuration, null)
        {

        }

        protected MvpRabbitMQBase(RabbitMQConfiguration configuration, RabbitMQQueueOptions options)
        {
            _brokerConfiguration = configuration ?? new RabbitMQConfiguration
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _queueOptions = options ?? new RabbitMQQueueOptions
            {
                RoutingKey = "mvp24hours-queue"
            };

            _factory = new ConnectionFactory()
            {
                HostName = _brokerConfiguration.HostName,
                Port = _brokerConfiguration.Port,
                UserName = _brokerConfiguration.UserName,
                Password = _brokerConfiguration.Password
            };
        }

        protected IModel CreateChannel()
        {
            try
            {
                _connection ??= _factory.CreateConnection();
                var channel = _connection.CreateModel();
                channel.QueueDeclare(queue: _queueOptions.Queue ?? string.Empty,
                                     durable: _queueOptions.Durable,
                                     exclusive: _queueOptions.Exclusive,
                                     autoDelete: _queueOptions.AutoDelete,
                                     arguments: _queueOptions.Arguments);
                return channel;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #region [ IDisposable ]

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _channel = null;
                _connection = null;
            }
        }

        #endregion

    }
}
