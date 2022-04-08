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
        private readonly string keyString = $"stringtest-{StringHelper.GenerateKey(5)}";
        private readonly string keyObject = $"objecttest-{StringHelper.GenerateKey(5)}";
        private readonly Startup startup;

        public Test3CacheRepositoryTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public void SetContentCache()
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
            repo.SetString(keyString, content);
        }

        [Fact, Priority(2)]
        public void GetString()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.SetString(keyString, "Test");
            string content = repo.GetString(keyString);
            Assert.True(content.HasValue());
        }

        [Fact, Priority(3)]
        public void RemoveString()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Remove(keyString);
            string content = repo.GetString(keyString);
            Assert.True(string.IsNullOrEmpty(content));
        }

        [Fact, Priority(4)]
        public void SetObjectContentCache()
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
            repo.Set(keyObject, customer);
        }

        [Fact, Priority(5)]
        public void GetObject()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Set(keyObject, new Customer { });
            var customer = repo.Get(keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void RemoveObject()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCache<Customer>>();
            repo.Remove(keyObject);
            var customer = repo.Get(keyObject);
            Assert.True(customer == null);
        }
    }
}
