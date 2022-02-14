//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.SQLServer.Test.Support.Data;
using Mvp24Hours.Application.SQLServer.Test.Support.Entities;
using Mvp24Hours.Application.SQLServer.Test.Support.Enums;
using Mvp24Hours.Application.SQLServer.Test.Support.Services;
using Mvp24Hours.Core.Helpers;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;
using System.Collections.Generic;

namespace Mvp24Hours.Application.SQLServer.Test.Setup
{
    public class Startup
    {
        public IServiceProvider Initialize(bool canLoadData = true)
        {
            var serviceProvider = ConfigureServices();

            // ensure database
            var db = serviceProvider.GetService<DataContext>();
            db.Database?.EnsureCreated();

            // load data
            if (canLoadData)
            {
                LoadData(serviceProvider);
            }
            return serviceProvider;
        }

        public void Cleanup(IServiceProvider serviceProvider)
        {
            // ensure database drop
            var db = serviceProvider?.GetService<DataContext>();
            if (db != null)
            {
                db.Database.EnsureDeleted();
                db.Dispose();
            }
        }

        private IServiceProvider ConfigureServices()
        {
#if InMemory
            var services = new ServiceCollection();
            services.AddDbContext<DataContext>(options =>
                options
                    .UseInMemoryDatabase(StringHelper.GenerateKey(10)));
#else
            var services = new ServiceCollection()
                .AddSingleton(ConfigurationHelper.AppSettings);

            services.AddDbContext<DataContext>(options =>
                options
                    .UseSqlServer(ConfigurationHelper.AppSettings.GetConnectionString("DataContext")
                        .Format(StringHelper.GenerateKey(10))));
#endif
            services.AddMvp24HoursDbContext<DataContext>(options: options =>
            {
                options.MaxQtyByQueryPage = 100;
                options.TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            });
            services.AddMvp24HoursRepository();

            // register my services
            services.AddScoped<CustomerService, CustomerService>();
            services.AddScoped<ContactService, ContactService>();
            services.AddScoped<CustomerPagingService, CustomerPagingService>();

            return services.BuildServiceProvider();
        }

        private void LoadData(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService<CustomerService>();
            List<Customer> customers = new();
            for (int i = 1; i <= 10; i++)
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
    }
}
