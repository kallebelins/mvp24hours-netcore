//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;

namespace Mvp24Hours.Extensions
{
    public static class RabbitMQServiceExtensions
    {
        /// <summary>
        /// Add pipeline engine
        /// </summary>
        public static IServiceCollection AddMvp24HoursRabbitMQ(this IServiceCollection services)
        {
            services.AddMvp24HoursLogging();
            return services;
        }
    }
}
