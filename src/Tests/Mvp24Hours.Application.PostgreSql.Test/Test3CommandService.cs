//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.PostgreSql.Test.Support.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Support.Helpers;
using Mvp24Hours.Application.PostgreSql.Test.Support.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System.Collections.Generic;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.PostgreSql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test3CommandService
    {
        public Test3CommandService()
        {
            StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void Create_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customer = new Customer
            {
                Name = "Test 1",
                Active = true
            };
            service.Add(customer);
            Assert.True(service.GetById(customer.Id).HasData());
        }
        [Fact, Priority(2)]
        public void Create_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            List<Customer> customers = new();
            for (int i = 2; i <= 10; i++)
            {
                customers.Add(new Customer
                {
                    Name = $"Test {i}",
                    Active = true
                });
            }
            service.Add(customers);
            Assert.True(service.GetByCount(x => x.Active).GetDataValue() > 0);
        }
        [Fact, Priority(3)]
        public void Update_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customer = service.GetById(1)
                .GetDataFirstOrDefault() as Customer;
            if (customer != null)
            {
                customer.Name = "Test Updated";
                service.Modify(customer);
                customer = service.GetById(customer.Id)
                    .GetDataFirstOrDefault() as Customer;
            }
            Assert.True(customer != null && customer.Name == "Test Updated");
        }
        [Fact, Priority(4)]
        public void Update_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            List<Customer> customers = new();
            for (int i = 2; i <= 10; i++)
            {
                customers.Add(new Customer
                {
                    Id = i,
                    Name = $"Test {i} Updated",
                    Active = false
                });
            }
            service.Modify(customers);
            int count = service.GetByCount(x => !x.Active)
                .GetDataValue();
            Assert.True(count > 0);
        }
        [Fact, Priority(5)]
        public void Delete_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(1, 0);
            var customer = service.List(paging)
                .GetDataFirstOrDefault() as Customer;
            if (customer != null)
            {
                service.RemoveById(customer.Id);
                customer = service.GetById(customer.Id)
                    .GetDataFirstOrDefault() as Customer;
            }
            Assert.True(customer == null);
        }
        [Fact, Priority(6)]
        public void Delete_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customers = service.List().Data;
            service.Remove(customers);
            int count = service.ListCount().GetDataValue();
            Assert.True(count == 0);
        }
    }
}
