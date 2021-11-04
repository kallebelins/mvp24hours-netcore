//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Data.MongoDb.Test.Data;
using Mvp24Hours.Infrastructure.Helpers;

namespace Mvp24Hours.Infrastructure.Data.MongoDb.Test.Helpers
{
    public class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            // register db context
            services.AddScoped(options =>
            {
                return new Mvp24HoursMongoDbContext("customers", ConfigurationHelper.GetSettings("ConnectionStrings:CustomerMongoContext"));
            });

            // register services
            services.AddScoped<IUnitOfWork>(x => new UnitOfWork());
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // register my services
            services.AddScoped<CustomerService, CustomerService>();

            ServiceProviderHelper.SetProvider(services.BuildServiceProvider());
        }
    }
}
