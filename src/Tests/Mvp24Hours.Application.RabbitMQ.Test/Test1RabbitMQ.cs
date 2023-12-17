//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.RabbitMQ;
using System;
using System.Threading.Tasks;
using Testcontainers.RabbitMq;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.RabbitMQ.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test1RabbitMQ : IAsyncLifetime
    {
        #region [ Container ]
        private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithExposedPort(5672)
            .WithUsername("guest")
            .WithPassword("guest")
            .WithCleanUp(true)
            .Build();

        public async Task InitializeAsync()
            => await _rabbitMqContainer.StartAsync().ConfigureAwait(false);

        public async Task DisposeAsync()
            => await _rabbitMqContainer.DisposeAsync().ConfigureAwait(false);
        #endregion

        #region [ Fields ]
        private IServiceProvider serviceProvider;
        #endregion

        #region [ Configure ]
        public Test1RabbitMQ() { }

        private void Setup()
        {
            var services = new ServiceCollection();
            services.AddMvp24HoursRabbitMQ(
                typeof(CustomerConsumer).Assembly,
                connectionOptions =>
                {
                    connectionOptions.ConnectionString = _rabbitMqContainer.GetConnectionString();
                    connectionOptions.DispatchConsumersAsync = true;
                    connectionOptions.RetryCount = 3;
                },
                clientOptions =>
                {
                    clientOptions.MaxRedeliveredCount = 1;
                }
            );
            services.AddScoped<CustomerConsumer, CustomerConsumer>();
            serviceProvider = services.BuildServiceProvider();
        }
        #endregion


        [Fact, Priority(1)]
        public void CreateProducer()
        {
            Setup();
            // arrange
            var client = serviceProvider.GetService<MvpRabbitMQClient>();

            // act
            string result = client.Publish(new CustomerEvent
            {
                Id = 1,
                Name = "Test 1",
                Active = true
            }, typeof(CustomerEvent).Name);

            // assert
            Assert.True(result.HasValue());
        }

        [Fact, Priority(2)]
        public void CreateConsumer()
        {
            Setup();
            var client = serviceProvider.GetService<MvpRabbitMQClient>();

            // arrange
            client.Publish(new CustomerEvent
            {
                Id = 2,
                Name = "Test 2",
                Active = true
            }, typeof(CustomerEvent).Name);


            // act
            client.Consume();

            // assert
            Assert.True(true);
        }
    }
}
