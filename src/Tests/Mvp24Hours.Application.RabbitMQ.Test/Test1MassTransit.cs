using MassTransit;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Common;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers.MassTransit;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test1MassTransit
    {

        [Fact, Priority(1)]
        public async Task Create_Producer()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                await busControl.Publish(new CustomerEvent
                {
                    Id = 99,
                    Name = "Test 1",
                    Active = true
                });
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        [Fact, Priority(2)]
        public async Task Create_Consumer()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint(EventBusConstants.CustomerQueue, e =>
                {
                    e.Consumer<MTCustomerConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                await Task.Run(() => Task.Delay(5000));
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
