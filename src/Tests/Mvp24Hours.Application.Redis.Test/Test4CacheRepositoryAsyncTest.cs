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
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test4CacheRepositoryAsyncTest
    {
        private readonly string _keyString = $"string_test-{StringHelper.GenerateKey(5)}";
        private readonly string _keyObject = $"object_test-{StringHelper.GenerateKey(5)}";
        private readonly Startup startup;

        public Test4CacheRepositoryAsyncTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public async Task Add_String_Cache_Async()
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

            var repo = serviceProvider.GetService<IRepositoryCacheAsync<Customer>>();
            await repo.SetStringAsync(_keyString, content);
        }

        [Fact, Priority(2)]
        public async Task Get_String_Async()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCacheAsync<Customer>>();
            await repo.SetStringAsync(_keyString, "Test");
            string content = await repo.GetStringAsync(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public async Task Remove_String_Async()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCacheAsync<Customer>>();
            await repo.RemoveAsync(_keyString);
            string content = await repo.GetStringAsync(_keyString);
            Assert.True(string.IsNullOrEmpty(content));
        }

        [Fact, Priority(4)]
        public async Task Add_Object_Cache_Async()
        {
            var serviceProvider = startup.Initialize();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            var repo = serviceProvider.GetService<IRepositoryCacheAsync<Customer>>();
            await repo.SetAsync(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public async Task Get_Object_Async()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCacheAsync<Customer>>();
            await repo.SetAsync(_keyObject, new Customer { });
            var customer = await repo.GetAsync(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public async Task Remove_Object_Async()
        {
            var serviceProvider = startup.Initialize();
            var repo = serviceProvider.GetService<IRepositoryCacheAsync<Customer>>();
            await repo.RemoveAsync(_keyObject);
            var customer = await repo.GetAsync(_keyObject);
            Assert.True(customer == null);
        }
    }
}
