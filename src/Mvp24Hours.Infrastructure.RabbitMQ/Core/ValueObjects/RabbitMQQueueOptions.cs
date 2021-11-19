using RabbitMQ.Client;
using System.Collections.Generic;

namespace Mvp24Hours.Core.ValueObjects.RabbitMQ
{
    public sealed class RabbitMQQueueOptions
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public bool AutoAck { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
        public IBasicProperties BasicProperties { get; set; }
    }
}
