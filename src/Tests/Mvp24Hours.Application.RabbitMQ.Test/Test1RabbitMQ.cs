using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.RabbitMQ.Test.Setup;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.RabbitMQ;
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
            var serviceProvider = startup.Initialize();
            var client = serviceProvider.GetService<MvpRabbitMQClient>();

            // act
            string result = client.Publish(new CustomerEvent
            {
                Id = 99,
                Name = "Test 1",
                Active = true
            }, typeof(CustomerEvent).Name);

            // assert
            Assert.True(result.HasValue());
        }

        [Fact, Priority(2)]
        public void Create_Consumer()
        {
            // arrange
            var serviceProvider = startup.Initialize();

            // act
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            while (!source.IsCancellationRequested)
            {
                var client = serviceProvider.GetService<MvpRabbitMQClient>();
                client.Consume();
            }

            // assert
            Assert.True(true);
        }
    }
}
