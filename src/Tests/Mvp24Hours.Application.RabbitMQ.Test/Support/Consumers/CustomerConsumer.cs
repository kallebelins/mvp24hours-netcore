using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using System;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers
{
    public class CustomerConsumer : IMvpRabbitMQConsumerAsync
    {
        public string RoutingKey => typeof(CustomerConsumer).Name;

        public string QueueName => typeof(CustomerConsumer).Name;

        public async Task ReceivedAsync(object message, string token)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "customer-consumer-received", $"dto:{message?.ToSerialize()}|token:{token}");
            await Task.CompletedTask;
        }

        public async Task FailureAsync(Exception exception, string token)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "customer-consumer-failure", exception, $"token:{token}");
            // perform handling for integration failures in RabbitMQ
            // write to temp table, send email, create specialized log, etc.
            await Task.CompletedTask;
        }

        public async Task RejectedAsync(object message, string token)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "customer-consumer-rejected", $"dto:{message?.ToSerialize()}|token:{token}");
            // we tried to consume the resource for 3 times, in this case as we did not treat it, we will disregard
            await Task.CompletedTask;
        }
    }
}
