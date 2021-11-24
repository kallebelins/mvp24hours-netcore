//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.MongoDb.Test.Support.Services;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;

namespace Mvp24Hours.Application.MongoDb.Test.Support.Helpers
{
    public static class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddMvp24HoursMongoDb("customers", ConfigurationHelper.GetSettings("ConnectionStrings:CustomerMongoContext"));

            // register my services
            services.AddScoped<CustomerService, CustomerService>();

            services.UseMvp24Hours();
        }
    }
}
