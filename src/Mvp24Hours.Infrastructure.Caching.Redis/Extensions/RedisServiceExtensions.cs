//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Helpers;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mvp24Hours.Extensions
{
    public static class RedisServiceExtensions
    {
        /// <summary>
        /// See settings at: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
        /// </summary>
        public static IServiceCollection AddMvp24HoursRedisCache(this IServiceCollection services, IConfiguration configuration = null)
        {
            services.AddMvp24HoursLogging();

            ConfigurationOptions redisConfiguration = null;

            if (configuration == null)
            {
                redisConfiguration = ConfigurationHelper.GetSettings<ConfigurationOptions>("Mvp24Hours:Infrastructure:Redis");
            }
            else
            {
                redisConfiguration = configuration.GetSection("Mvp24Hours:Infrastructure:Redis").Get<ConfigurationOptions>();
            }

            if (redisConfiguration == null)
            {
                throw new ArgumentNullException("Redis configuration not defined. [Mvp24Hours:Infrastructure:Redis]");
            }

            var hosts = ConfigurationHelper.GetSettings<List<string>>("Mvp24Hours:Infrastructure:Redis:Hosts");

            if (hosts == null)
            {
                throw new ArgumentNullException("Redis hosts configuration not defined. [Mvp24Hours:Infrastructure:Redis:Hosts]");
            }

            foreach (var h in hosts)
            {
                redisConfiguration.EndPoints.Add(h);
            }

            string instanceName = ConfigurationHelper.GetSettings("Mvp24Hours:Infrastructure:Redis:InstanceName")
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
        public static IServiceCollection AddMvp24HoursRedisCache(this IServiceCollection services, string connectionStringName, string instanceName = null, IConfiguration configuration = null)
        {
            services.AddMvp24HoursLogging();

            string connectionString = null;

            if (configuration == null)
            {
                connectionString = ConfigurationHelper.AppSettings.GetConnectionString(connectionStringName);
            }
            else
            {
                connectionString = configuration.GetConnectionString(connectionStringName);
            }

            if (!connectionString.HasValue())
            {
                throw new ArgumentNullException("Connection strings not defined. [ConnectionStrings:ConnectionName]");
            }

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = $"{instanceName ?? Assembly.GetEntryAssembly().GetName().Name.Replace(".", "_")}_".ToLower();
            });

            return services;
        }
    }
}
