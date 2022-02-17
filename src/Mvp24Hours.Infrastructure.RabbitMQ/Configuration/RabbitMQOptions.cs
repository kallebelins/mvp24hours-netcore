//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Enums;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Mvp24Hours.Infrastructure.RabbitMQ.Configuration
{
    [Serializable]
    public sealed class RabbitMQOptions
    {
        public string ConnectionString { get; set; } = "amqp://guest:guest@localhost:5672";
        public RabbitMQConnection ConnectionConfiguration { get; set; }
        public string Exchange { get; set; } = "amq.direct";
        public MvpRabbitMQExchangeType ExchangeType { get; set; } = MvpRabbitMQExchangeType.direct;
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public bool AutoAck { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
        public IBasicProperties BasicProperties { get; set; }
    }
}
