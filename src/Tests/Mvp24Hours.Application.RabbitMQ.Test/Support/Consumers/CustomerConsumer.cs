using Mvp24Hours.Application.RabbitMQ.Test.Support.Common;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Infrastructure.RabbitMQ;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers
{
    public class CustomerConsumer : MvpRabbitMQConsumer<CustomerEvent>
    {
        public CustomerConsumer()
            : base(EventBusConstants.CustomerQueue)
        {
        }

        public override Task Received(CustomerEvent message)
        {
            Trace.WriteLine($"Received customer {message?.Name}");
            return Task.FromResult(false);
        }
    }
}
