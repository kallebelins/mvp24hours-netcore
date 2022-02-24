//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Pipe;
using System;

namespace Mvp24Hours.Application.Pipe.Test.Setup
{
    public class Startup
    {
        public IServiceProvider Initialize()
        {
            var services = new ServiceCollection()
                            .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursPipeline(options =>
            {
                options.IsBreakOnFail = false;
            });

            return services.BuildServiceProvider();
        }

        public IServiceProvider InitializeWithFactory()
        {
            var services = new ServiceCollection()
                           .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursPipeline(factory: (_) =>
            {
                var pipeline = new Pipeline();
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
