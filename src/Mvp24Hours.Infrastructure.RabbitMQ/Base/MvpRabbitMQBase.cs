//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Logging;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using RabbitMQ.Client;
using System;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public abstract class MvpRabbitMQBase : IDisposable
    {
        #region [ Properties / Fields ]
        private readonly RabbitMQOptions _queueOptions;
        private readonly ConnectionFactory _factory;

        private ILoggingService _logging;
        private IModel _channel;
        private IConnection _connection;

        protected virtual RabbitMQOptions Options => _queueOptions;
        protected virtual IConnection Connection
        {
            get
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection ??= _factory.CreateConnection();
                }
                return _connection;
            }
        }
        protected virtual IModel Channel
        {
            get
            {
                if (_channel == null || _channel.IsClosed)
                {
                    _channel = Connection.CreateModel();
                    _channel.ExchangeDeclare(
                        exchange: _queueOptions.Exchange,
                        type: _queueOptions.ExchangeType.ToString(),
                        durable: _queueOptions.Durable,
                        autoDelete: _queueOptions.AutoDelete,
                        arguments: _queueOptions.Arguments
                    );
                    _channel.QueueDeclare(
                        queue: _queueOptions.Queue ?? string.Empty,
                        durable: _queueOptions.Durable,
                        exclusive: _queueOptions.Exclusive,
                        autoDelete: _queueOptions.AutoDelete,
                        arguments: _queueOptions.Arguments
                    ); ;
                }
                return _channel;
            }
        }
        protected virtual bool DispatchConsumersAsync => false;
        protected virtual ILoggingService Logging => _logging ??= LoggingService.GetLoggingService();
        #endregion

        #region [ Ctors ]
        protected MvpRabbitMQBase(RabbitMQOptions options, string queueName, string routingKey = null)
            : this(BuilderOptions(options, queueName, routingKey: routingKey))
        {
        }

        protected MvpRabbitMQBase(RabbitMQOptions options)
        {
            _queueOptions = options ?? throw new ArgumentNullException(nameof(options), "Options is required.");

            if (_queueOptions.ConnectionString.HasValue())
            {
                _factory = new ConnectionFactory()
                {
                    Uri = new Uri(_queueOptions.ConnectionString),
                    DispatchConsumersAsync = DispatchConsumersAsync
                };
            }
            else if (_queueOptions.ConnectionConfiguration != null)
            {
                var config = _queueOptions.ConnectionConfiguration;
                _factory = new ConnectionFactory()
                {
                    HostName = config.HostName,
                    Port = config.Port,
                    UserName = config.UserName,
                    Password = config.Password,
                    DispatchConsumersAsync = DispatchConsumersAsync
                };
            }
            else
            {
                throw new ArgumentNullException(nameof(options), "Connection string/configuration is required.");
            }
        }
        #endregion

        #region [ Methods ]
        protected static RabbitMQOptions BuilderOptions(RabbitMQOptions options, string queueName, string routingKey = null)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "Options is required.");
            }

            if (!queueName.HasValue())
            {
                options.Queue = queueName;
            }
            if (routingKey.HasValue())
            {
                options.RoutingKey = routingKey;
            }
            return options;
        }
        #endregion

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
