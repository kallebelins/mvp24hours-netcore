﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    public class MvpRabbitMQClient
    {
        #region [ Properties / Fields ]
        private readonly RabbitMQClientOptions _options;
        private readonly IMvpRabbitMQConnection _connection;
        private readonly Dictionary<string, IModel> _channels;

        protected virtual RabbitMQClientOptions Options => _options;
        protected virtual IMvpRabbitMQConnection Connection => _connection;
        protected virtual Dictionary<string, IModel> Channels => _channels;
        #endregion

        #region [ Ctors ]
        [ActivatorUtilitiesConstructor]
        public MvpRabbitMQClient(IOptions<RabbitMQClientOptions> options, IMvpRabbitMQConnection connection)
            : this(options?.Value, connection)
        {
        }

        public MvpRabbitMQClient(RabbitMQClientOptions options, IMvpRabbitMQConnection connection)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-ctor");
            this._options = options ?? throw new ArgumentNullException(nameof(options));
            this._connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this._channels = new Dictionary<string, IModel>();
        }
        #endregion

        #region [ Methods]
        public virtual string Publish(object message, string routingKey, string tokenDefault = null)
        {
            try
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-publish-start", $"token:{tokenDefault}");

                if (!routingKey.HasValue() && !Options.RoutingKey.HasValue())
                {
                    throw new ArgumentNullException(nameof(routingKey), "RoutingKey is required if you do not provide a default route in the configuration.");
                }

                if (!tokenDefault.HasValue())
                {
                    tokenDefault = Guid.NewGuid().ToString();
                }

                if (!Connection.IsConnected)
                {
                    Connection.TryConnect();
                }

                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(Connection.Options.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        TelemetryHelper.Execute(TelemetryLevels.Warning, "rabbitmq-client-publish-tryconnect-waitandretry", $"Could not publish: {tokenDefault} after {time.TotalSeconds:n1}s ({ex.Message})");
                    });

                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-publish-channel-creating", $"token:{tokenDefault}");

                using (var channel = Connection.CreateModel())
                {
                    ExchangeDeclare(channel, Options);

                    var bsEvent = message.ToBusinessEvent(tokenDefault: tokenDefault);
                    TelemetryHelper.Execute(TelemetryLevels.Information, "rabbitmq-client-publish-token", tokenDefault);
                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-publish-body-created", $"token:{tokenDefault}");
                    var body = Encoding.UTF8.GetBytes(bsEvent.ToSerialize(JsonHelper.JsonBusinessEventSettings()));

                    policy.Execute(() =>
                    {
                        var properties = channel.CreateBasicProperties();
                        properties.DeliveryMode = 2;
                        properties.ContentType = "application/json";
                        properties.CorrelationId = tokenDefault;
                        SetRedeliveredCount(properties, 0);

                        TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-publish-sending", $"token:{tokenDefault}");

                        channel.ConfirmSelect();
                        channel.BasicPublish(exchange: Options.Exchange,
                                         routingKey: routingKey ?? Options.RoutingKey,
                                         basicProperties: Options.BasicProperties ?? properties,
                                         body: body);
                        channel.WaitForConfirmsOrDie();
                        channel.ConfirmSelect();
                    });

                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-publish-success", $"token:{tokenDefault}");
                    TelemetryHelper.Execute(TelemetryLevels.Information, "rabbitmq-client-publish-success", $"token:{tokenDefault}");
                }

                return tokenDefault;
            }
            catch (Exception ex)
            {
                TelemetryHelper.Execute(TelemetryLevels.Error, "rabbitmq-client-publish-failure", ex);
                throw;
            }
            finally
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-publish-end", $"token:{tokenDefault}");
            }
        }

        public virtual void Consume()
        {
            try
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-start");

                if (!TypeConsumers.AnySafe())
                {
                    throw new ArgumentException("We didn't find consumers registered through the configuration.");
                }

                if (Connection.Options.DispatchConsumersAsync)
                {
                    if (TypeConsumers.AnySafe(x => x.InheritsOrImplements(typeof(IMvpRabbitMQConsumerSync))))
                    {
                        throw new ArgumentException("DispatchConsumersAsync is enabled, so register only classes that implement the IMvpRabbitMQConsumerAsync interface.");
                    }
                }
                else
                {
                    if (TypeConsumers.AnySafe(x => x.InheritsOrImplements(typeof(IMvpRabbitMQConsumerAsync))))
                    {
                        throw new ArgumentException("DispatchConsumersAsync is disabled, so register only classes that implement the IMvpRabbitMQConsumerSync interface.");
                    }
                }

                foreach (var customer in GetConsumers())
                {
                    var channel = CreateConsumerChannel(QueueBind, customer.RoutingKey, customer.QueueName);

                    // dead letter
                    if (Options.DeadLetter != null)
                    {
                        ExchangeDeclare(channel, Options.DeadLetter);
                        QueueDeclare(channel, Options.DeadLetter, $"dead-letter-{customer.QueueName}");
                        QueueBind(channel, Options.DeadLetter, customer.RoutingKey, $"dead-letter-{customer.QueueName}");
                    }

                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-event", $"channel-number:{channel.ChannelNumber}");
                    IBasicConsumer _event = null;

                    if (Connection.Options.DispatchConsumersAsync)
                    {
                        var _eventAsync = new AsyncEventingBasicConsumer(channel);
                        _eventAsync.Received += async (sender, e) =>
                        {
                            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received");
                            await HandleConsumeAsync(e, channel, (IMvpRabbitMQConsumerAsync)customer);
                        };
                        _event = _eventAsync;
                    }
                    else
                    {
                        var _eventSync = new EventingBasicConsumer(channel);
                        _eventSync.Received += (sender, e) =>
                        {
                            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received");
                            HandleConsume(e, channel, (IMvpRabbitMQConsumerSync)customer);
                        };
                        _event = _eventSync;
                    }

                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-basic-qos", $"prefetchSize:{0}|prefetchCount:{1}|global: false");
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-basic", $"queue:{customer.QueueName ?? Options.QueueName ?? string.Empty}|autoAck: false");
                    channel.BasicConsume(queue: customer.QueueName ?? Options.QueueName ?? string.Empty,
                         autoAck: false,
                         consumer: _event);
                }
            }
            catch (Exception ex)
            {
                TelemetryHelper.Execute(TelemetryLevels.Error, "rabbitmq-consumer-failure", ex);
                throw;
            }
            finally
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-end");
            }
        }

        private IModel CreateConsumerChannel(Action<IModel, RabbitMQClientOptions, string, string> action, string routingKey, string queueName)
        {
            string queueKeyName = queueName ?? Options.QueueName ?? string.Empty;
            IModel channel = null;

            if (Channels.ContainsKey(queueKeyName))
            {
                channel = Channels[queueKeyName];
            }

            if (channel == null || channel.IsClosed)
            {
                if (!Connection.IsConnected)
                {
                    Connection.TryConnect();
                }

                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-channel-creating");
                channel = Connection.CreateModel();

                ExchangeDeclare(channel, Options);
                QueueDeclare(channel, Options, queueKeyName);

                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-channel-created", $"channel-number:{channel.ChannelNumber}");

                channel.CallbackException += (sender, ea) =>
                {
                    if (!channel.IsOpen)
                        channel.Dispose();
                    channel = CreateConsumerChannel(action, routingKey, queueName);
                    TelemetryHelper.Execute(TelemetryLevels.Warning, "rabbitmq-channel-recreating", $"channel-number:{channel.ChannelNumber}");
                };

                action?.Invoke(channel, Options, routingKey, queueName);

                if (!Channels.ContainsKey(queueKeyName))
                    Channels.Add(queueKeyName, channel);
            }
            return channel;
        }

        private void ExchangeDeclare(IModel channelCtx, RabbitMQOptions optionsCtx)
        {
            if (channelCtx == null || optionsCtx == null) return;

            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-channel-exchange-declaring", $"exchange:{optionsCtx.Exchange}|type:{optionsCtx.ExchangeType}|durable:{optionsCtx.Durable}|autoDelete:{optionsCtx.AutoDelete}");
            channelCtx.ExchangeDeclare(
                exchange: optionsCtx.Exchange,
                type: optionsCtx.ExchangeType.ToString(),
                durable: optionsCtx.Durable,
                autoDelete: optionsCtx.AutoDelete,
                arguments: optionsCtx.ExchangeArguments
            );
        }

        private void QueueDeclare(IModel channelCtx, RabbitMQOptions optionsCtx, string queueName = null)
        {
            if (channelCtx == null || optionsCtx == null) return;

            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-channel-queue-setting", $"queue:{queueName ?? optionsCtx.QueueName ?? string.Empty}|durable:{optionsCtx.Durable}|exclusive:{optionsCtx.Exclusive}|autoDelete:{optionsCtx.AutoDelete}");
            channelCtx.QueueDeclare(
                queue: queueName ?? optionsCtx.QueueName ?? string.Empty,
                durable: optionsCtx.Durable,
                exclusive: optionsCtx.Exclusive,
                autoDelete: optionsCtx.AutoDelete,
                arguments: optionsCtx.QueueArguments
            );
        }

        private void QueueBind(IModel channelCtx, RabbitMQOptions optionsCtx, string routingKey, string queueName)
        {
            if (channelCtx == null || optionsCtx == null) return;

            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-queue-bind", $"queue:{queueName ?? optionsCtx.QueueName ?? string.Empty}|exchange:{optionsCtx.Exchange}|routingKey:{routingKey ?? optionsCtx.RoutingKey}");
            channelCtx.QueueBind(queue: queueName ?? optionsCtx.QueueName ?? string.Empty,
                                    exchange: optionsCtx.Exchange,
                                    routingKey: routingKey ?? optionsCtx.RoutingKey);
        }

        private void HandleConsume(BasicDeliverEventArgs e, IModel channel, IMvpRabbitMQConsumerSync consumerSync)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-start");
            string token = null;
            object data = null;
            int redeliveredCount = 0;
            IBasicProperties properties = null;
            try
            {
                properties = e.BasicProperties;
                redeliveredCount = GetRedeliveredCount(properties);

                IBusinessEvent bsEvent = ExtractBodyToBusinessEvent(e);
                token = bsEvent.Token;
                TelemetryHelper.Execute(TelemetryLevels.Information, "rabbitmq-consumer-received-token", token);
                TelemetryHelper.Execute(TelemetryLevels.Information, "rabbitmq-consumer-received-redelivered", $"count:{redeliveredCount}|token:{token}");

                data = bsEvent.GetDataObject();
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-dispatching-start", $"token:{token}");
                try
                {
                    consumerSync.Received(data, token);
                }
                finally
                {
                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-dispatching-end", $"token:{token}");
                }
                BasicAck(e, channel);
            }
            catch (Exception ex)
            {
                try
                {
                    TelemetryHelper.Execute(TelemetryLevels.Error, "rabbitmq-consumer-received-failure", ex);

                    if (consumerSync is IMvpRabbitMQConsumerRecoverySync)
                        (consumerSync as IMvpRabbitMQConsumerRecoverySync).Failure(ex, token);

                    if (redeliveredCount < Options.MaxRedeliveredCount)
                    {
                        TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-redelivered", $"count:{redeliveredCount}");
                        SetRedeliveredCount(properties, redeliveredCount);
                        channel.BasicPublish(e.Exchange, e.RoutingKey, properties, e.Body);
                        BasicAck(e, channel);
                    }
                    else
                    {
                        TelemetryHelper.Execute(TelemetryLevels.Error, "rabbitmq-consumer-received-reject", $"redelivered-count:{redeliveredCount}");
                        if (consumerSync is IMvpRabbitMQConsumerRecoverySync)
                            (consumerSync as IMvpRabbitMQConsumerRecoverySync).Rejected(data, token);
                        BasicNack(e, channel);
                    }
                }
                catch (Exception)
                {
                    BasicNack(e, channel);
                }
            }
            finally
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-end");
            }
        }

        private async Task HandleConsumeAsync(BasicDeliverEventArgs e, IModel channel, IMvpRabbitMQConsumerAsync consumerAsync)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-start");
            string token = null;
            object data = null;
            int redeliveredCount = 0;
            IBasicProperties properties = null;
            try
            {
                properties = e.BasicProperties;
                redeliveredCount = GetRedeliveredCount(properties);

                IBusinessEvent bsEvent = ExtractBodyToBusinessEvent(e);
                token = bsEvent.Token;
                TelemetryHelper.Execute(TelemetryLevels.Information, "rabbitmq-consumer-received-token", token);
                TelemetryHelper.Execute(TelemetryLevels.Information, "rabbitmq-consumer-received-redelivered", $"count:{redeliveredCount}|token:{token}");

                data = bsEvent.GetDataObject();

                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-dispatching-start", $"token:{token}");
                try
                {
                    await consumerAsync.ReceivedAsync(data, token);
                }
                finally
                {
                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-dispatching-end", $"token:{token}");
                }
                BasicAck(e, channel);
            }
            catch (Exception ex)
            {
                try
                {
                    TelemetryHelper.Execute(TelemetryLevels.Error, "rabbitmq-consumer-received-failure", ex);
                    if (consumerAsync is IMvpRabbitMQConsumerRecoveryAsync)
                        await (consumerAsync as IMvpRabbitMQConsumerRecoveryAsync).FailureAsync(ex, token);

                    if (redeliveredCount < Options.MaxRedeliveredCount)
                    {
                        // requeue
                        TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-redelivered", $"count:{redeliveredCount}");
                        SetRedeliveredCount(properties, redeliveredCount);
                        channel.BasicPublish(e.Exchange, e.RoutingKey, properties, e.Body);
                        BasicAck(e, channel);
                    }
                    else
                    {
                        TelemetryHelper.Execute(TelemetryLevels.Error, "rabbitmq-consumer-received-reject", $"redelivered-count:{redeliveredCount}");
                        if (consumerAsync is IMvpRabbitMQConsumerRecoveryAsync)
                            await (consumerAsync as IMvpRabbitMQConsumerRecoveryAsync).RejectedAsync(data, token);
                        BasicNack(e, channel);
                    }
                }
                catch (Exception)
                {
                    BasicNack(e, channel);
                }
            }
            finally
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-end");
            }
        }

        private int GetRedeliveredCount(IBasicProperties properties)
        {
            int result = (int?)properties.Headers?["x-redelivered-count"] ?? 0;
            return result + 1;
        }

        private void SetRedeliveredCount(IBasicProperties properties, int count)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-retryaccount");
            properties.Headers ??= new Dictionary<string, object>();
            properties.Headers["x-redelivered-count"] = count;
        }

        private void BasicAck(BasicDeliverEventArgs e, IModel channel)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-basicack");
            channel.BasicAck(e.DeliveryTag, false);
        }

        private void BasicNack(BasicDeliverEventArgs e, IModel channel)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-basicnack");
            channel.BasicNack(e.DeliveryTag, false, false);
        }

        private IBusinessEvent ExtractBodyToBusinessEvent(BasicDeliverEventArgs e)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-body-extract-start");
            try
            {
                var body = e.Body.ToArray();
                string messageString = Encoding.UTF8.GetString(body);
                IBusinessEvent bsEvent = messageString.ToDeserialize<IBusinessEvent>(JsonHelper.JsonBusinessEventSettings());
                return bsEvent;
            }
            finally
            {
                TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-consumer-received-body-extract-end");
            }
        }

        #endregion

        #region [ Static ]
        static List<IMvpRabbitMQConsumer> _consumers;
        static List<IMvpRabbitMQConsumer> GetConsumers()
        {
            if (_consumers == null)
            {
                var consumersSync = TypeConsumers
                    .Where(x => x.InheritsOrImplements(typeof(IMvpRabbitMQConsumerSync)))
                    .Select(t => (IMvpRabbitMQConsumerAsync)Activator.CreateInstance(t))
                    .ToList();

                var consumersAsync = TypeConsumers
                    .Where(x => x.InheritsOrImplements(typeof(IMvpRabbitMQConsumerAsync)))
                    .Select(t => (IMvpRabbitMQConsumerAsync)Activator.CreateInstance(t))
                    .ToList();

                _consumers = new List<IMvpRabbitMQConsumer>();
                _consumers.AddRange(consumersSync);
                _consumers.AddRange(consumersAsync);
            }
            return _consumers;
        }
        static readonly List<Type> TypeConsumers = new();

        public static void Register<T>() where T : class, IMvpRabbitMQConsumer
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-consumer-register");
            Register(typeof(T));
        }
        public static void Register(Type consumerType)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-consumer-register");
            if (consumerType == null)
            {
                throw new ArgumentNullException(nameof(consumerType));
            }
            TypeConsumers.Add(consumerType);
            _consumers = null;
        }
        public static void Unregister<T>() where T : class, IMvpRabbitMQConsumer
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-consumer-unregister");
            TypeConsumers
                .FindAll(x => x.InheritsOrImplements(typeof(T)))
                ?.ForEach(item => TypeConsumers.Remove(item));
        }
        public static void Unregister(Type consumerType)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "rabbitmq-client-consumer-unregister");
            if (consumerType == null)
            {
                throw new ArgumentNullException(nameof(consumerType));
            }
            TypeConsumers.Remove(consumerType);
            _consumers = null;
        }
        #endregion
    }
}
