//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class RedisServiceExtensions
    {
        /// <summary>
        /// See settings at: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
        /// </summary>
        public static IServiceCollection AddMvp24HoursRedisCache(this IServiceCollection services)
        {
            var redisConfiguration = ConfigurationHelper.GetSettings<ConfigurationOptions>("Mvp24Hours:Persistence:Redis");

            if (redisConfiguration == null)
            {
                throw new ArgumentNullException("Redis configuration not defined. [Mvp24Hours:Persistence:Redis]");
            }

            var hosts = ConfigurationHelper.GetSettings<List<string>>("Mvp24Hours:Persistence:Redis:Hosts");

            if (hosts == null)
            {
                throw new ArgumentNullException("Redis hosts configuration not defined. [Mvp24Hours:Persistence:Redis:Hosts]");
            }

            foreach (var h in hosts)
            {
                redisConfiguration.EndPoints.Add(h);
            }

            string instanceName = ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:Redis:InstanceName")
                ?? Assembly.GetEntryAssembly().GetName().Name.Replace(".", "_");

            services.AddDistributedRedisCache(options =>
            {
                options.ConfigurationOptions = redisConfiguration;
                options.InstanceName = $"{instanceName}".ToLower();
            });

            return services;
        }

        /// <summary>
        /// See settings at: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
        /// </summary>
        public static IServiceCollection AddMvp24HoursRedisCache(this IServiceCollection services, string connectionStringName, string instanceName = null)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = ConfigurationHelper.AppSettings.GetConnectionString(connectionStringName);
                options.InstanceName = $"{instanceName ?? Assembly.GetEntryAssembly().GetName().Name.Replace(".", "_")}_".ToLower();
            });

            return services;
        }
    }
}
