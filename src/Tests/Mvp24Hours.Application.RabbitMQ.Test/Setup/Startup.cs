//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;

namespace Mvp24Hours.Application.RabbitMQ.Test.Setup
{
    public class Startup
    {
        public IServiceProvider InitializeProducer()
        {
            var services = new ServiceCollection()
                            .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursRabbitMQ(options =>
            {
                options.ConnectionString = ConfigurationHelper.GetSettings("ConnectionStrings:HostBus");
            });

            services.AddScoped<CustomerProducer, CustomerProducer>();

            return services.BuildServiceProvider();
        }

        public IServiceProvider InitializeConsumer()
        {
            var services = new ServiceCollection()
                            .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursRabbitMQ(options =>
            {
                options.ConnectionString = ConfigurationHelper.GetSettings("ConnectionStrings:HostBus");
            });

            services.AddScoped<CustomerConsumer, CustomerConsumer>();

            return services.BuildServiceProvider();
        }

    }
}
