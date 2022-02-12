using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.RabbitMQ.Test.Setup;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using System;
using System.Threading;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.RabbitMQ.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test1RabbitMQ
    {
        private readonly Startup startup;

        /// <summary>
        /// Initialize
        /// </summary>
        public Test1RabbitMQ()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public void Create_Producer()
        {
            // arrange
            var serviceProvider = startup.InitializeProducer();
            var producer = serviceProvider.GetService<CustomerProducer>();

            // act
            producer.Publish(new CustomerEvent
            {
                Id = 99,
                Name = "Test 1",
                Active = true
            });

            // assert
            Assert.True(true);
        }

        [Fact, Priority(2)]
        public void Create_Consumer()
        {
            // arrange
            var serviceProvider = startup.InitializeConsumer();

            // act
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            while (!source.IsCancellationRequested)
            {
                var consumer = serviceProvider.GetService<CustomerConsumer>();
                consumer.Consume();
            }

            // assert
            Assert.True(true);
        }
    }
}
