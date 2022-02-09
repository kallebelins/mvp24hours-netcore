//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.RabbitMQ;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using System;

namespace Mvp24Hours.Extensions
{
    public static class RabbitMQServiceExtensions
    {
        /// <summary>
        /// Add rabbitmq
        /// </summary>
        public static IServiceCollection AddMvp24HoursRabbitMQ(this IServiceCollection services,
            Action<RabbitMQOptions> options = null)
        {
            services.AddMvp24HoursLogging();

            if (options != null)
            {
                services.Configure(options);
            }
            else
            {
                services.Configure<RabbitMQOptions>(options => { });
            }

            return services;
        }

        /// <summary>
        /// Add hosted service
        /// </summary>
        public static IServiceCollection AddMvp24HoursHostedService(this IServiceCollection services,
            Action<RabbitMQHostedOptions> options = null)
        {
            if (options != null)
            {
                services.Configure(options);
            }
            else
            {
                services.Configure<RabbitMQHostedOptions>(options => { });
            }
            services.AddHostedService<MvpRabbitMQHostedService>();
            return services;
        }
    }
}
