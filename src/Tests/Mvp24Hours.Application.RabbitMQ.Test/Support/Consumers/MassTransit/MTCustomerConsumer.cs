using MassTransit;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers.MassTransit
{
    public class MTCustomerConsumer : IConsumer<CustomerEvent>
    {
        public Task Consume(ConsumeContext<CustomerEvent> context)
        {
            Trace.WriteLine($"Message received for {context.Message?.Name}");
            return Task.FromResult(false);
        }
    }
}
