//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Redis.Test.Support.Entities;
using Mvp24Hours.Application.Redis.Test.Support.Helpers;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test3CacheRepositoryTest
    {
        private readonly string _keyString = "string_test";
        private readonly string _keyObject = "object_test";

        public Test3CacheRepositoryTest()
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

            var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
            repo.SetString(_keyString, content);
        }

        [Fact, Priority(2)]
        public void Get_String()
        {
            var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
            string content = repo.GetString(_keyString);
            Assert.True(!string.IsNullOrEmpty(content));
        }

        [Fact, Priority(3)]
        public void Remove_String()
        {
            var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
            repo.Remove(_keyString);
            string content = repo.GetString(_keyString);
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

            var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
            repo.Set(_keyObject, customer);
        }

        [Fact, Priority(5)]
        public void Get_Object()
        {
            var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
            var customer = repo.Get(_keyObject);
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void Remove_Object()
        {
            var repo = ServiceProviderHelper.GetService<IRepositoryCache<Customer>>();
            repo.Remove(_keyObject);
            var customer = repo.Get(_keyObject);
            Assert.True(customer == null);
        }
    }
}
