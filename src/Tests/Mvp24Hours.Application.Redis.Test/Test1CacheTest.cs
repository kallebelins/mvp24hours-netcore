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
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Redis.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test1CacheTest
    {
        private readonly string _keyString = $"string_test-{StringHelper.GenerateKey(5)}";
        private readonly string _keyObject = $"object_test-{StringHelper.GenerateKey(5)}";
        private readonly Startup startup;

        public Test1CacheTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public void Set_String()
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
            cache.SetString(_keyString, content);

            // assert
            Assert.True(cache.GetString(_keyString).HasValue());
        }

        [Fact, Priority(2)]
        public void Get_String()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();

            // act
            cache.SetString(_keyString, "Test");
            string content = cache.GetString(_keyString);

            // assert
            Assert.True(content.HasValue());
        }

        [Fact, Priority(3)]
        public void Remove_String()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();

            //  act
            cache.Remove(_keyString);

            // assert
            string content = cache.GetString(_keyString);
            Assert.True(!content.HasValue());
        }

        [Fact, Priority(4)]
        public void Set_Object()
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
            cache.SetObject(_keyObject, customer);

            // assert
            var result = cache.GetObject<Customer>(_keyObject);
            Assert.True(result != null);
        }

        [Fact, Priority(5)]
        public void Get_Object()
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
            cache.SetObject(_keyObject, customer);

            //  act
            customer = cache.GetObject<Customer>(_keyObject);

            // assert
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void Remove_Object()
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
            cache.SetObject(_keyObject, customer);

            //  act
            cache.Remove(_keyObject);

            // assert
            customer = cache.GetObject<Customer>(_keyObject);
            Assert.True(customer == null);
        }
    }
}
