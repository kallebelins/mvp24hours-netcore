//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Consumers;
using Mvp24Hours.Application.RabbitMQ.Test.Support.Dto;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.RabbitMQ;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
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

            services.AddScoped<MvpRabbitMQProducer<CustomerEvent>, CustomerProducer>();

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

            services.AddScoped<MvpRabbitMQConsumer<CustomerEvent>, CustomerConsumer>();

            return services.BuildServiceProvider();
        }

    }
}
