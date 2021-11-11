//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.MySql.Test.Data;
using Mvp24Hours.Application.MySql.Test.Services;
using Mvp24Hours.Application.MySql.Test.Services.Async;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;

namespace Mvp24Hours.Application.MySql.Test.Helpers
{
    public class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options => 
                options.UseMySQL(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

            services.AddMvp24HoursDbService<DataContext>();

            // register my services
            services.AddScoped<CustomerService, CustomerService>();
            services.AddScoped<ContactService, ContactService>();

            ServiceProviderHelper.SetProvider(services.BuildServiceProvider());

            // ensure database
            var db = ServiceProviderHelper.GetService<DataContext>();
            db.Database.EnsureCreated();
        }

        public static void ConfigureServicesAsync()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options =>
                options.UseMySQL(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

            services.AddMvp24HoursDbAsyncService<DataContext>();

            // register my services
            services.AddScoped<CustomerServiceAsync, CustomerServiceAsync>();
            services.AddScoped<ContactServiceAsync, ContactServiceAsync>();

            ServiceProviderHelper.SetProvider(services.BuildServiceProvider());

            // ensure database
            var db = ServiceProviderHelper.GetService<DataContext>();
            db.Database.EnsureCreated();
        }
    }
}
