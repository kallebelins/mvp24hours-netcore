//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.MongoDb.Test.Helpers;
using Mvp24Hours.Application.MongoDb.Test.Services;
using Mvp24Hours.Application.SQLServer.Test.Entities;
using Mvp24Hours.Infrastructure.Helpers;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MongoDb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ServiceCommandTest
    {
        public ServiceCommandTest()
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
            service.SaveChanges();

            customer = service.GetById(1);

            Assert.True(customer != null);
        }

        [Fact, Priority(2)]
        public void Update_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            var customer = service.GetById(1);

            if (customer != null)
            {
                customer.Name = "Test Updated";

                service.Modify(customer);

                customer = service.GetById(1);
            }

            Assert.True(customer != null && customer.Name == "Test Updated");
        }

        [Fact, Priority(3)]
        public void Delete_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            service.RemoveById(1);

            var customer = service.GetById(1);

            Assert.True(customer == null);
        }

    }
}
