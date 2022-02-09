//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;

namespace Mvp24Hours.Application.Pipe.Test.Support.Helpers
{
    public static class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursNotification();
            services.AddMvp24HoursPipeline();

            services.UseMvp24Hours();
        }

        public static void ConfigureServicesAsync()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursNotification();
            services.AddMvp24HoursPipelineAsync();

            services.UseMvp24Hours();
        }
    }
}
