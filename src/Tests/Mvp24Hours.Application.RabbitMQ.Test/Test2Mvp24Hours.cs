using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Helpers;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Threading;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test2Mvp24Hours
    {
        [Fact, Priority(1)]
        public void Create_Producer()
        {
            StartupHelper.ConfigureProducerServices();
            var producer = ServiceProviderHelper.GetService<CustomerProducer>();
            producer.Publish(new CustomerEvent
            {
                Id = 99,
                Name = "Test 1",
                Active = true
            });
            Assert.True(true);
        }

        [Fact, Priority(2)]
        public void Create_Consumer()
        {
            StartupHelper.ConfigureConsumerServices();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            while (!source.IsCancellationRequested)
            {
                var consumer = ServiceProviderHelper.GetService<CustomerConsumer>();
                consumer.Consume();
            }
            Assert.True(true);
        }
    }
}
