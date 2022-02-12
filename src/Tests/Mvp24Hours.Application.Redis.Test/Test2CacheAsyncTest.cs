//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.Redis.Test.Setup;
using Mvp24Hours.Application.Redis.Test.Support.Entities;
using Mvp24Hours.Core.Helpers;
using Mvp24Hours.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test2CacheAsyncTest
    {
        private readonly string _keyString = $"string_test-{StringHelper.GenerateKey(5)}";
        private readonly string _keyObject = $"object_test-{StringHelper.GenerateKey(5)}";
        private readonly Startup startup;

        public Test2CacheAsyncTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public async Task Set_String_Async()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            string content = customer.ToSerialize();

            // act
            await cache.SetStringAsync(_keyString, content);

            // assert
            var result = await cache.GetStringAsync(_keyString);
            Assert.True(result.HasValue());
        }

        [Fact, Priority(2)]
        public async Task Get_String_Async()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();

            // act
            await cache.SetStringAsync(_keyString, "Test");
            string content = await cache.GetStringAsync(_keyString);

            // assert
            Assert.True(content.HasValue());
        }

        [Fact, Priority(3)]
        public async Task Remove_String_Async()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();

            //  act
            await cache.RemoveAsync(_keyString);

            // assert
            string content = await cache.GetStringAsync(_keyString);
            Assert.True(!content.HasValue());
        }

        [Fact, Priority(4)]
        public async Task Set_Object_Async()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };

            //  act
            await cache.SetObjectAsync(_keyObject, customer);

            // assert
            var result = await cache.GetObjectAsync<Customer>(_keyObject);
            Assert.True(result != null);
        }

        [Fact, Priority(5)]
        public async Task Get_Object_Async()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            await cache.SetObjectAsync(_keyObject, customer);

            //  act
            customer = await cache.GetObjectAsync<Customer>(_keyObject);

            // assert
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public async Task Remove_Object_Async()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            await cache.SetObjectAsync(_keyObject, customer);

            //  act
            await cache.RemoveAsync(_keyObject);

            // assert
            customer = await cache.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
