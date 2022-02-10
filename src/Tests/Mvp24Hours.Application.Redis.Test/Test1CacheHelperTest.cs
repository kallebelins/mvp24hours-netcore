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
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test1cacheTest
    {
        private readonly string _keyString = "string_test";
        private readonly string _keyObject = "object_test";
        private readonly IDistributedCache cache;

        public Test1cacheTest()
        {
            StartupHelper.ConfigureServices();
            cache = ServiceProviderHelper.GetService<IDistributedCache>();
        }

        [Fact, Priority(1)]
        public void Set_String()
        {
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            string content = customer.ToSerialize();
            DistributedCacheExtensions.SetString(cache, _keyString, content);
        }

        [Fact, Priority(2)]
        public void Get_String()
        {
            string content = cache.GetString(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public void Remove_String()
        {
            cache.Remove(_keyString);
            string content = cache.GetString(_keyString);
            Assert.True(string.IsNullOrEmpty(content));
        }

        [Fact, Priority(4)]
        public void Set_Object()
        {
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            cache.SetObject(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public void Get_Object()
        {
            var customer = cache.GetObject<Customer>(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void Remove_Object()
        {
            cache.Remove(_keyObject);
            var customer = cache.GetObject<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
