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
    public class RedisServiceTest
    {
        private readonly string _keyString = "string_test";
        private readonly string _keyObject = "object_test";

        public RedisServiceTest()
        {
            StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public async Task Set_String()
        {
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            string content = customer.ToSerialize();
            await RedisCacheHelper.SetStringAsync(_keyString, content);
        }

        [Fact, Priority(2)]
        public async Task Get_String()
        {
            string content = await RedisCacheHelper.GetStringAsync(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public async Task Remove_String()
        {
            await RedisCacheHelper.RemoveStringAsync(_keyString);
            string content = await RedisCacheHelper.GetStringAsync(_keyString);
            Assert.True(string.IsNullOrEmpty(content));
        }

        [Fact, Priority(4)]
        public async Task Set_Object()
        {
            var customer = new Customer
            {
                Oid = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test 1",
                Active = true
            };
            await RedisObjectCacheHelper.SetObjectAsync(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public async Task Get_Object()
        {
            var customer = await RedisObjectCacheHelper.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public async Task Remove_Object()
        {
            await RedisCacheHelper.RemoveStringAsync(_keyObject);
            var customer = await RedisObjectCacheHelper.GetObjectAsync<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
