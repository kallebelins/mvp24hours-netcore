//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;

namespace Mvp24Hours.Application.RabbitMQ.Test.Support.Helpers
{
    public static class StartupHelper
    {
        public static void ConfigureProducerServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddScoped<CustomerProducer, CustomerProducer>();

            services.UseMvp24Hours();
        }

        public static void ConfigureConsumerServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddScoped<CustomerConsumer, CustomerConsumer>();

            services.UseMvp24Hours();
        }

    }
}
