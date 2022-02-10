//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Caching.Distributed;
using Mvp24Hours.Application.Redis.Test.Support.Entities;
using Mvp24Hours.Application.Redis.Test.Support.Helpers;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test2CacheHelperAsyncTest
    {
        private readonly string _keyString = "string_test";
        private readonly string _keyObject = "object_test";
        private readonly IDistributedCache cache;

        public Test2CacheHelperAsyncTest()
        {
            StartupHelper.ConfigureServices();
            cache = ServiceProviderHelper.GetService<IDistributedCache>();
        }

        [Fact, Priority(1)]
        public async Task Set_String_Async()
        {
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            string content = customer.ToSerialize();
            await DistributedCacheExtensions.SetStringAsync(cache, _keyString, content);
        }

        [Fact, Priority(2)]
        public async Task Get_String_Async()
        {
            string content = await cache.GetStringAsync(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public async Task Remove_String_Async()
        {
            await cache.RemoveAsync(_keyString);
            string content = await cache.GetStringAsync(_keyString);
            Assert.True(string.IsNullOrEmpty(content));
        }

        [Fact, Priority(4)]
        public async Task Set_Object_Async()
        {
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            await cache.SetObjectAsync(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public async Task Get_Object_Async()
        {
            var customer = await cache.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public async Task Remove_Object_Async()
        {
            await cache.RemoveAsync(_keyObject);
            var customer = await cache.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
