//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.MongoDb.Test.Support.Services;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;

namespace Mvp24Hours.Application.MongoDb.Test.Support.Helpers
{
    public static class StartupHelper
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursDbContext(options =>
            {
                options.DatabaseName = "customers";
                options.ConnectionString = ConfigurationHelper.GetSettings("ConnectionStrings:CustomerMongoContext");
            });
            services.AddMvp24HoursRepository();

            // register my services
            services.AddScoped<CustomerService, CustomerService>();

            var provider = services.BuildServiceProvider();
            ServiceProviderHelper.SetProvider(provider);
            return provider;
        }
    }
}
