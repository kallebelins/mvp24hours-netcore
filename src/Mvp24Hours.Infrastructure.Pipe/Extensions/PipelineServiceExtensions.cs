//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Pipe.Configuration;
using System;

namespace Mvp24Hours.Extensions
{
    public static class PipelineServiceExtensions
    {
        /// <summary>
        /// Add pipeline engine
        /// </summary>
        public static IServiceCollection AddMvp24HoursPipeline(this IServiceCollection services, Action<PipelineOptions> options = null)
        {
            services.AddMvp24HoursLogging();
            services.AddMvp24HoursNotification();

            services.AddScoped<IPipeline, Pipeline>();

            if (options != null)
            {
                services.Configure(options);
            }
            else
            {
                services.Configure<PipelineOptions>(options =>
                {
                    options.IsBreakOnFail = false;
                });
            }

            return services;
        }

        /// <summary>
        /// Add pipeline engine async
        /// </summary>
        public static IServiceCollection AddMvp24HoursPipelineAsync(this IServiceCollection services, Action<PipelineAsyncOptions> options = null)
        {
            services.AddMvp24HoursLogging();
            services.AddMvp24HoursNotification();

            services.AddScoped<IPipelineAsync, PipelineAsync>();

            if (options != null)
            {
                services.Configure(options);
            }
            else
            {
                services.Configure<PipelineAsyncOptions>(options =>
                {
                    options.IsBreakOnFail = false;
                });
            }

            return services;
        }
    }
}
