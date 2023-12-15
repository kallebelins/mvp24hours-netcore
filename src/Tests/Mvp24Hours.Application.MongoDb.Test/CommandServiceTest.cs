//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Mvp24Hours.Application.MongoDb.Test.Support.Entities;
using Mvp24Hours.Application.MongoDb.Test.Support.Helpers;
using Mvp24Hours.Application.MongoDb.Test.Support.Services;
using Mvp24Hours.Extensions;
using System;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MongoDb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class CommandServiceTest
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ObjectId oid = ObjectId.GenerateNewId();

        public CommandServiceTest()
        {
            serviceProvider = StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void CreateCustomer()
        {
            var service = serviceProvider.GetService<CustomerService>();

            service.Add(new Customer
            {
                Oid = oid,
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            });

            var result = service.GetById(oid);

            Assert.True(result.HasData());
        }

        [Fact, Priority(2)]
        public void UpdateCustomer()
        {
            var service = serviceProvider.GetService<CustomerService>();

            var customer = new Customer
            {
                Oid = oid,
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            service.Add(customer);

            customer.Name = "Test Updated";

            service.Modify(customer);

            var boCustomer = service.GetById(oid);

            Assert.True(boCustomer != null && boCustomer.Data?.Name == "Test Updated");
        }

        [Fact, Priority(3)]
        public void DeleteCustomer()
        {
            var service = serviceProvider.GetService<CustomerService>();

            service.RemoveById(oid);

            var result = service.GetById(oid);

            Assert.True(!result.HasData());
        }

    }
}
