//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.RabbitMQ;
using System;
using System.Threading;

namespace Mvp24Hours.Extensions
{
    public static class RabbitMQServiceExtensions
    {
        /// <summary>
        /// Add rabbitmq
        /// </summary>
        public static IServiceCollection AddMvp24HoursRabbitMQ(this IServiceCollection services)
        {
            services.AddMvp24HoursLogging();
            return services;
        }

        /// <summary>
        /// Add hosted service
        /// </summary>
        public static IServiceCollection AddMvp24HoursHostedService(this IServiceCollection services, TimerCallback callback, object state = null, TimeSpan? dueTime = null, TimeSpan? period = null)
        {
            services.AddHostedService<MvpRabbitMQHostedService>((x) => new MvpRabbitMQHostedService(callback, state, dueTime, period));
            return services;
        }
    }
}
