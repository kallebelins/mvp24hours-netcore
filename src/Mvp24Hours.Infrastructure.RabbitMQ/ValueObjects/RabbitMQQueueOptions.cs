using System;
using System.Collections.Generic;
using System.Text;

namespace Mvp24Hours.Infrastructure.RabbitMQ.ValueObjects
{
    public sealed class RabbitMQQueueOptions
    {
        public string Queue { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
    }
}
