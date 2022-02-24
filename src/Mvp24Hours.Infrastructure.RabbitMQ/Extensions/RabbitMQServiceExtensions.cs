//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.RabbitMQ;
using Mvp24Hours.Infrastructure.RabbitMQ.Configuration;
using Mvp24Hours.Infrastructure.RabbitMQ.Core.Contract;
using System;
using System.Linq;
using System.Reflection;

namespace Mvp24Hours.Extensions
{
    public static class RabbitMQServiceExtensions
    {
        /// <summary>
        /// Add rabbitmq
        /// </summary>
        public static IServiceCollection AddMvp24HoursRabbitMQ(this IServiceCollection services,
            Assembly assemblyConsumers,
            Action<RabbitMQConnectionOptions> connectionOptions = null,
            Action<RabbitMQClientOptions> clientOptions = null)
        {
            if (assemblyConsumers == null)
            {
                throw new ArgumentNullException(nameof(assemblyConsumers));
            }

            var types = assemblyConsumers.GetExportedTypes()
                .Where(t => t.InheritsOrImplements(typeof(IMvpRabbitMQConsumer)))
                .ToList();

            foreach (var type in types)
            {
                MvpRabbitMQClient.Register(type);
            }

            if (connectionOptions != null)
            {
                services.Configure(connectionOptions);
            }
            else
            {
                services.Configure<RabbitMQConnectionOptions>(connectionOptions => { });
            }

            services.AddSingleton<IMvpRabbitMQConnection, MvpRabbitMQConnection>();

            if (clientOptions != null)
            {
                services.Configure(clientOptions);
            }
            else
            {
                services.Configure<RabbitMQClientOptions>(options => { });
            }

            services.AddSingleton<MvpRabbitMQClient, MvpRabbitMQClient>();

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
