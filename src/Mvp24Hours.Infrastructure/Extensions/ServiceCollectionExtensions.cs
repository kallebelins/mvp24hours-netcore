//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Mappings;
using Mvp24Hours.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Validations;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Create a ServiceProvider
        /// </summary>
        public static IServiceCollection BuildMvp24HoursProvider(this IServiceCollection services, ServiceProviderOptions options = null)
        {
            ServiceProviderHelper.SetProvider(services.BuildServiceProvider(options));
            return services;
        }

        /// <summary>
        /// Adds essential services
        /// </summary>
        public static IServiceCollection AddMvp24HoursNotification(this IServiceCollection services)
        {
            // notification
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped(typeof(IValidatorNotify<>), typeof(ValidatorNotify<>));

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
