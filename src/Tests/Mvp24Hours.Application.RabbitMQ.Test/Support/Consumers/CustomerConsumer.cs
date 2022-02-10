using Microsoft.Extensions.Options;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Infrastructure.RabbitMQ;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using System.Diagnostics;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers
{
    public class CustomerConsumer : MvpRabbitMQConsumer<CustomerEvent>
    {
        public CustomerConsumer(IOptions<RabbitMQOptions> options)
            : base(options)
        {
        }

        public override void Received(object message)
        {
            Trace.WriteLine($"Received customer {(message as CustomerEvent)?.Name}");
        }
    }
}
