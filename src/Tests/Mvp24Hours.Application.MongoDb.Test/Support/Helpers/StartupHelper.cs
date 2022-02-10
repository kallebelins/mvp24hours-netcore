//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.MongoDb.Test.Support.Services;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;

namespace Mvp24Hours.Application.MongoDb.Test.Support.Helpers
{
    public static class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursMongoDb(options =>
            {
                options.DatabaseName = "customers";
                options.ConnectionString = ConfigurationHelper.GetSettings("ConnectionStrings:CustomerMongoContext");
            });

            // register my services
            services.AddScoped<CustomerService, CustomerService>();

            services.UseMvp24Hours();
        }
    }
}
