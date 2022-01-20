//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.Mappings;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Logging;
using System.Reflection;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Mvp24Hours essential
        /// </summary>
        public static IServiceCollection AddMvp24HoursEssential(this IServiceCollection services, IConfiguration configuration = null)
        {
            services.AddSingleton(services);
            services.AddMvp24HoursConfiguration(configuration);
            services.AddMvp24HoursLogging();
            services.AddMvp24HoursNotification();
            return services;
        }

        internal static IServiceCollection AddMvp24HoursConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration != null)
            {
                ConfigurationHelper.SetConfiguration(configuration);
            }
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
                services.AddSingleton<ILoggingService>((x) => LoggingService.GetLoggingService());
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
        /// Add mapping services, use Assembly.GetExecutingAssembly()
        /// </summary>
        public static IServiceCollection AddMvp24HoursMapService(this IServiceCollection services, Assembly assemblyMap)
        {
            if (assemblyMap == null)
            {
                throw new System.ArgumentNullException(nameof(assemblyMap), "Assembly Map is required.");
            }

            Assembly local = assemblyMap;
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
