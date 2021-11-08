//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Bson;
using Mvp24Hours.Application.MongoDb.Test.Entities;
using Mvp24Hours.Application.MongoDb.Test.Helpers;
using Mvp24Hours.Application.MongoDb.Test.Services;
using Mvp24Hours.Infrastructure.Helpers;
using System;
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
        private readonly ObjectId _oid = ObjectId.GenerateNewId();

        public ServiceCommandTest()
        {
            StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void Create_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            service.Add(new Customer
            {
                Oid = _oid,
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            });

            var customer = service.GetById(_oid);

            Assert.True(customer != null);
        }

        [Fact, Priority(2)]
        public void Update_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            var customer = service.GetById(_oid);

            if (customer != null)
            {
                customer.Name = "Test Updated";

                service.Modify(customer);

                customer = service.GetById(_oid);
            }

            Assert.True(customer != null && customer.Name == "Test Updated");
        }

        [Fact, Priority(3)]
        public void Delete_Customer()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            service.RemoveById(_oid);

            var customer = service.GetById(_oid);

            Assert.True(customer == null);
        }

    }
}
