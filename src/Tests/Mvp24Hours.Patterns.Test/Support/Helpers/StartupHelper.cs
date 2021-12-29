//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Patterns.Test.Support.Data;
using Mvp24Hours.Patterns.Test.Support.Entities;
using Mvp24Hours.Patterns.Test.Support.Enums;
using Mvp24Hours.Patterns.Test.Support.Services;
using Mvp24Hours.Patterns.Test.Support.Services.Async;
using Mvp24Hours.Patterns.Test.Support.Validations;
using System.Collections.Generic;
using System.Reflection;

namespace Mvp24Hours.Patterns.Test.Support.Helpers
{
    public static class StartupHelper
    {
        public static void ConfigureServices(bool enableFluentValidation = false)
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

            services.AddMvp24HoursNotification();
            services.AddMvp24HoursDbService<DataContext>();

            if (enableFluentValidation)
            {
                services.AddSingleton<IValidator<Customer>, CustomerValidator>();
            }

            services.AddScoped<CustomerService, CustomerService>();

            services.UseMvp24Hours();

            services.AddMvp24HoursMapService(Assembly.GetExecutingAssembly());

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
                    Type = ContactType.CellPhone,
                    Active = true
                });
                customer.Contacts.Add(new Contact
                {
                    Description = $"test{i}@sample.com",
                    Type = ContactType.Email,
                    Active = true
                });
                customers.Add(customer);
            }
            service.Add(customers);
        }

        public static void ConfigureServicesAsync(bool enableFluentValidation = false)
        {
            var services = new ServiceCollection().AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")));

            services.AddMvp24HoursNotification();
            services.AddMvp24HoursDbAsyncService<DataContext>();

            if (enableFluentValidation)
            {
                services.AddSingleton<IValidator<Customer>, CustomerValidator>();
            }

            // register my services
            services.AddScoped<CustomerServiceAsync, CustomerServiceAsync>();

            services.UseMvp24Hours();

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
                    Type = ContactType.CellPhone,
                    Active = true
                });
                customer.Contacts.Add(new Contact
                {
                    Description = $"test{i}@sample.com",
                    Type = ContactType.Email,
                    Active = true
                });
                customers.Add(customer);
            }
            await service.AddAsync(customers);
        }
    }
}
