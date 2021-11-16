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
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Patterns.Test.Data;
using Mvp24Hours.Patterns.Test.Entities;
using Mvp24Hours.Patterns.Test.Services;
using Mvp24Hours.Patterns.Test.Services.Async;
using System.Collections.Generic;

namespace Mvp24Hours.Patterns.Test.Helpers
{
    public class StartupHelper
    {
        public static void ConfigureServices()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

            services.AddMvp24HoursDbService<DataContext>();


            services.BuildMvp24HoursProvider();

            // ensure database
            var db = ServiceProviderHelper.GetService<DataContext>();
            db.Database.EnsureCreated();
        }

        public static void LoadData()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            List<Customer> customers = new();
            for (int i = 1; i < 10; i++)
            {
                var customer = new Customer
                {
                    Name = $"Test {i}",
                    Active = true
                };
                customer.Contacts.Add(new Contact
                {
                    Description = $"202-555-014{i}",
                    Type = Enums.ContactType.CellPhone,
                    Active = true
                });
                customer.Contacts.Add(new Contact
                {
                    Description = $"test{i}@sample.com",
                    Type = Enums.ContactType.Email,
                    Active = true
                });
                customers.Add(customer);
            }
            service.Add(customers);
            service.SaveChanges();
        }

        public static void ConfigureServicesAsync()
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

            services.AddMvp24HoursDbAsyncService<DataContext>();

            // register my services
            services.AddScoped<CustomerServiceAsync, CustomerServiceAsync>();

            services.BuildMvp24HoursProvider();

            // ensure database
            var db = ServiceProviderHelper.GetService<DataContext>();
            db.Database.EnsureCreated();
        }

        public static async void LoadDataAsync()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            List<Customer> customers = new();
            for (int i = 1; i < 10; i++)
            {
                var customer = new Customer
                {
                    Name = $"Test {i}",
                    Active = true
                };
                customer.Contacts.Add(new Contact
                {
                    Description = $"202-555-014{i}",
                    Type = Enums.ContactType.CellPhone,
                    Active = true
                });
                customer.Contacts.Add(new Contact
                {
                    Description = $"test{i}@sample.com",
                    Type = Enums.ContactType.Email,
                    Active = true
                });
                customers.Add(customer);
            }
            await service.AddAsync(customers);
            await service.SaveChangesAsync();
        }
    }
}
