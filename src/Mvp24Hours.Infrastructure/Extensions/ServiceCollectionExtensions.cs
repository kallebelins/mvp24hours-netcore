//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.Mappings;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Logging;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Mvp24Hours essential
        /// </summary>
        public static IServiceCollection AddMvp24Hours(this IServiceCollection services)
        {
            services.AddMvp24HoursLogging();
            services.AddMvp24HoursNotification();
            return services;
        }

        /// <summary>
        /// Add logging
        /// </summary>
        public static IServiceCollection AddMvp24HoursLogging(this IServiceCollection services)
        {
            // notification
            if (!services.Exists<ILoggingService>())
            {
                services.AddScoped<ILoggingService, LoggingService>();
            }

            return services;
        }

        /// <summary>
        /// Add notification pattern
        /// </summary>
        public static IServiceCollection AddMvp24HoursNotification(this IServiceCollection services)
        {
            // notification
            if (!services.Exists<INotificationContext>())
            {
                services.AddScoped<INotificationContext, NotificationContext>();
            }

            return services;
        }

        /// <summary>
        /// Add mapping services
        /// </summary>
        public static IServiceCollection AddMvp24HoursMapService(this IServiceCollection services, Assembly assemblyMap)
        {
            Assembly local = assemblyMap ?? Assembly.GetExecutingAssembly();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile(local));
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
