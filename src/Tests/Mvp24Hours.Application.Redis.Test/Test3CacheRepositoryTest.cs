//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.Redis.Test.Setup;
using Mvp24Hours.Application.Redis.Test.Support.Entities;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Helpers;
using Mvp24Hours.Extensions;
using System;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test3CacheRepositoryTest
    {
        private readonly string _keyString = $"string_test-{StringHelper.GenerateKey(5)}";
        private readonly string _keyObject = $"object_test-{StringHelper.GenerateKey(5)}";
        private readonly Startup startup;

        public Test3CacheRepositoryTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public void Set_String()
        {
            var serviceProvider = startup.Initialize();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            string content = customer.ToSerialize();

            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.SetString(_keyString, content);
        }

        [Fact, Priority(2)]
        public void Get_String()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.SetString(_keyString, "Test");
            string content = repo.GetString(_keyString);
            Assert.True(content.HasValue());
        }

        [Fact, Priority(3)]
        public void Remove_String()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Remove(_keyString);
            string content = repo.GetString(_keyString);
            Assert.True(string.IsNullOrEmpty(content));
        }

        [Fact, Priority(4)]
        public void Set_Object()
        {
            var serviceProvider = startup.Initialize();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Set(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public void Get_Object()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Set(_keyObject, new Customer { });
            var customer = repo.Get(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void Remove_Object()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Remove(_keyObject);
            var customer = repo.Get(_keyObject);
            Assert.True(customer == null);
        }
    }
}
