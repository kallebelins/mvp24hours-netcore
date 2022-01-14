//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
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
    public class Test1CacheHelperTest
    {
        private readonly string _keyString = "string_test";
        private readonly string _keyObject = "object_test";

        public Test1CacheHelperTest()
        {
            StartupHelper.ConfigureServices();
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
            CacheHelper.SetString(_keyString, content);
        }

        [Fact, Priority(2)]
        public void Get_String()
        {
            string content = CacheHelper.GetString(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public void Remove_String()
        {
            CacheHelper.RemoveString(_keyString);
            string content = CacheHelper.GetString(_keyString);
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
            ObjectCacheHelper.SetObject(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public void Get_Object()
        {
            var customer = ObjectCacheHelper.GetObject<Customer>(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void Remove_Object()
        {
            CacheHelper.RemoveString(_keyObject);
            var customer = ObjectCacheHelper.GetObject<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
