//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;

namespace Mvp24Hours.Application.Pipe.Test.Helpers
{
    public class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursNotification();
            services.AddMvp24HoursPipeline();

            services.BuildMvp24HoursProvider();
        }

        public static void ConfigureServicesAsync()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursNotification();
            services.AddMvp24HoursPipelineAsync();

            services.BuildMvp24HoursProvider();
        }
    }
}
