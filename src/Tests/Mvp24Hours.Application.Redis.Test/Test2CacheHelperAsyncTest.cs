//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Redis.Test.Support.Entities;
using Mvp24Hours.Application.Redis.Test.Support.Helpers;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
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

        public Test2CacheHelperAsyncTest()
        {
            StartupHelper.ConfigureServices();
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
            await CacheAsyncHelper.SetStringAsync(_keyString, content);
        }

        [Fact, Priority(2)]
        public async Task Get_String_Async()
        {
            string content = await CacheAsyncHelper.GetStringAsync(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public async Task Remove_String_Async()
        {
            await CacheAsyncHelper.RemoveStringAsync(_keyString);
            string content = await CacheAsyncHelper.GetStringAsync(_keyString);
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
            await ObjectCacheAsyncHelper.SetObjectAsync(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public async Task Get_Object_Async()
        {
            var customer = await ObjectCacheAsyncHelper.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public async Task Remove_Object_Async()
        {
            await CacheAsyncHelper.RemoveStringAsync(_keyObject);
            var customer = await ObjectCacheAsyncHelper.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
