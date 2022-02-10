//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.MySql.Test.Support.Data;
using Mvp24Hours.Application.MySql.Test.Support.Entities;
using Mvp24Hours.Application.MySql.Test.Support.Helpers;
using Mvp24Hours.Application.MySql.Test.Support.Services.Async;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MySql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test4CommandServiceAsync
    {
        #region [ Ctor ]
        public Test4CommandServiceAsync()
        {
            var startup = new StartupHelper();
            startup.ConfigureServicesAsync();
        }

        [Fact, Priority(99)]
        public void Database_Ensure_Delete()
        {
            // ensure database drop
            var db = ServiceProviderHelper.GetService<DataContext>();
            if (db != null)
                Assert.True(db.Database.EnsureDeleted());
        }
        #endregion

        #region [ Actions ]
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
            customer = await service.GetByIdAsync(customer.Id)
                .GetDataValueAsync();
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
            int count = await service.GetByCountAsync(x => x.Active)
                .GetDataValueAsync();
            Assert.True(count > 0);
        }
        [Fact, Priority(3)]
        public async Task Update_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0);
            var customer = await service.ListAsync(paging)
                .GetDataValueAsync()
                .FirstOrDefaultAsync();
            if (customer != null)
            {
                customer.Name = "Test Updated";
                await service.ModifyAsync(customer);
                customer = await service.GetByIdAsync(customer.Id)
                    .GetDataValueAsync();
            }
            Assert.True(customer != null && customer.Name == "Test Updated");
        }
        [Fact, Priority(4)]
        public async Task Update_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            var customers = await service.ListAsync(paging)
                .GetDataValueAsync();
            foreach (var item in customers)
            {
                item.Active = false;
            }

            await service.ModifyAsync(customers);
            int count = await service.GetByCountAsync(x => !x.Active)
                .GetDataValueAsync();
            Assert.True(count > 0);
        }
        [Fact, Priority(5)]
        public async Task Delete_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0);
            var customer = await service.ListAsync(paging)
                .GetDataValueAsync()
                .FirstOrDefaultAsync();
            if (customer != null)
            {
                await service.RemoveByIdAsync(customer.Id);
                customer = await service.GetByIdAsync(customer.Id)
                    .GetDataValueAsync();
            }
            Assert.True(customer == null);
        }
        [Fact, Priority(6)]
        public async Task Delete_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var customers = (await service.ListAsync()).Data;
            await service.RemoveAsync(customers);
            int count = await service.ListCountAsync()
                .GetDataValueAsync();
            Assert.True(count == 0);
        }
        #endregion
    }
}
