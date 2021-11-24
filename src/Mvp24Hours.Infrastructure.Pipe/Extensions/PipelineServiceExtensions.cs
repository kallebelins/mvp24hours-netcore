//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Pipe;

namespace Mvp24Hours.Infrastructure.Extensions
{
    public static class PipelineServiceExtensions
    {
        /// <summary>
        /// Add pipeline engine
        /// </summary>
        public static IServiceCollection AddMvp24HoursPipeline(this IServiceCollection services, string token = null, bool isBreakOnFail = false)
        {
            services.AddMvp24HoursNotification();
            services.AddScoped<IPipeline>(x => new Pipeline(token, isBreakOnFail));
            return services;
        }

        /// <summary>
        /// Add pipeline engine async
        /// </summary>
        public static IServiceCollection AddMvp24HoursPipelineAsync(this IServiceCollection services, string token = null, bool isBreakOnFail = false)
        {
            services.AddMvp24HoursNotification();
            services.AddScoped<IPipelineAsync>(x => new PipelineAsync(token, isBreakOnFail));
            return services;
        }
    }
}
