//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.PostgreSql.Test.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Helpers;
using Mvp24Hours.Application.PostgreSql.Test.Services;
using Mvp24Hours.Application.PostgreSql.Test.Services.Async;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.PostgreSql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CommandServiceAsyncTest
    {
        public CommandServiceAsyncTest()
        {
            StartupHelper.ConfigureServicesAsync();
        }

        [Fact, Priority(1)]
        public async Task Create_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var customer = new Customer
            {
                Name = "Test 1",
                Active = true
            };
            await service.AddAsync(customer);
            await service.SaveChangesAsync();
            customer = await service.GetByIdAsync(customer.Id);
            Assert.True(customer != null);
        }
        [Fact, Priority(2)]
        public async Task Create_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            List<Customer> customers = new();
            for (int i = 2; i <= 10; i++)
            {
                customers.Add(new Customer
                {
                    Name = $"Test {i}",
                    Active = true
                });
            }
            await service.AddAsync(customers);
            await service.SaveChangesAsync();
            int count = await service.GetByCountAsync(x => x.Active);
            Assert.True(count > 0);
        }
        [Fact, Priority(3)]
        public async Task Update_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0);
            var customer = (await service.ListAsync(paging))?.FirstOrDefault();
            if (customer != null)
            {
                customer.Name = "Test Updated";
                await service.ModifyAsync(customer);
                await service.SaveChangesAsync();
                customer = await service.GetByIdAsync(customer.Id);
            }
            Assert.True(customer != null && customer.Name == "Test Updated");
        }
        [Fact, Priority(4)]
        public async Task Update_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            var customers = await service.ListAsync(paging);
            foreach (var item in customers)
                item.Active = false;
            await service.ModifyAsync(customers);
            await service.SaveChangesAsync();
            int count = await service.GetByCountAsync(x => !x.Active);
            Assert.True(count > 0);
        }
        [Fact, Priority(5)]
        public async Task Delete_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0);
            var customer = (await service.ListAsync(paging))?.FirstOrDefault();
            if (customer != null)
            {
                await service.RemoveByIdAsync(customer.Id);
                await service.SaveChangesAsync();
                customer = await service.GetByIdAsync(customer.Id);
            }
            Assert.True(customer == null);
        }
        [Fact, Priority(6)]
        public async Task Delete_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var customers = await service.ListAsync();
            await service.RemoveAsync(customers);
            await service.SaveChangesAsync();
            int count = await service.ListCountAsync();
            Assert.True(count == 0);
        }
    }
}
