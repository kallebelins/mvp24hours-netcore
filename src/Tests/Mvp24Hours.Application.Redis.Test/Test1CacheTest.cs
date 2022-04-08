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
        private readonly string keyString = $"stringtest-{StringHelper.GenerateKey(5)}";
        private readonly string keyObject = $"objecttest-{StringHelper.GenerateKey(5)}";
        private readonly Startup startup;

        public Test1CacheTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public void SetString()
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
            cache.SetString(keyString, content);

            // assert
            Assert.True(cache.GetString(keyString).HasValue());
        }

        [Fact, Priority(2)]
        public void GetString()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();

            // act
            cache.SetString(keyString, "Test");
            string content = cache.GetString(keyString);

            // assert
            Assert.True(content.HasValue());
        }

        [Fact, Priority(3)]
        public void RemoveString()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var cache = serviceProvider.GetService<IDistributedCache>();

            //  act
            cache.Remove(keyString);

            // assert
            string content = cache.GetString(keyString);
            Assert.True(!content.HasValue());
        }

        [Fact, Priority(4)]
        public void SetObject()
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
            cache.SetObject(keyObject, customer);

            // assert
            var result = cache.GetObject<Customer>(keyObject);
            Assert.True(result != null);
        }

        [Fact, Priority(5)]
        public void GetObject()
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
            cache.SetObject(keyObject, customer);

            //  act
            customer = cache.GetObject<Customer>(keyObject);

            // assert
            Assert.True(customer != null);
        }

        [Fact, Priority(6)]
        public void RemoveObject()
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
            cache.SetObject(keyObject, customer);

            //  act
            cache.Remove(keyObject);

            // assert
            customer = cache.GetObject<Customer>(keyObject);
            Assert.True(customer == null);
        }
    }
}
