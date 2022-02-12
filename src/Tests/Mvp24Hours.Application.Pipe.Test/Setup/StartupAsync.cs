//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Pipe;
using System;

namespace Mvp24Hours.Application.Pipe.Test.Setup
{
    public class StartupAsync
    {
        public IServiceProvider Initialize()
        {
            var services = new ServiceCollection()
                           .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursPipelineAsync(options =>
            {
                options.IsBreakOnFail = false;
            });

            return services.BuildServiceProvider();
        }

        public IServiceProvider InitializeWithFactory()
        {
            var services = new ServiceCollection()
                           .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursPipelineAsync(factory: (x) =>
            {
                var pipeline = new PipelineAsync(x.GetRequiredService<INotificationContext>());
                pipeline.AddInterceptors(input =>
                {
                    input.AddContent<int>("factory", 1);
                    System.Diagnostics.Trace.WriteLine("Interceptor factory.");
                }, Core.Enums.Infrastructure.PipelineInterceptorType.PostOperation);
                return pipeline;
            });

            return services.BuildServiceProvider();
        }

    }
}
