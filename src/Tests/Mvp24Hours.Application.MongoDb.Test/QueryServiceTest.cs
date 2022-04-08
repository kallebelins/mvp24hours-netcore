//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Mvp24Hours.Application.MongoDb.Test.Support.Entities;
using Mvp24Hours.Application.MongoDb.Test.Support.Helpers;
using Mvp24Hours.Application.MongoDb.Test.Support.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MongoDb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class QueryServiceTest
    {
        private readonly IServiceProvider serviceProvider;

        public QueryServiceTest()
        {
            serviceProvider = StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void CreateManyCustomers()
        {
            var service = serviceProvider.GetService<CustomerService>();
            for (int i = 0; i < 10; i++)
            {
                service.Add(new Customer
                {
                    Oid = ObjectId.GenerateNewId(),
                    Created = DateTime.Now,
                    Name = $"Test {i}",
                    Active = true
                });
            }
            var result = service.GetByCount(x => x.Active);
            Assert.True(result.GetDataValue() > 0);
        }

        [Fact, Priority(2)]
        public void GetFilterCustomerList()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var result = service.List();
            Assert.True(result.GetDataCount() > 0);
        }

        [Fact, Priority(3)]
        public void GetFilterCustomerListAny()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var result = service.ListAny();
            Assert.True(result.GetDataValue());
        }

        [Fact, Priority(4)]
        public void GetFilterCustomerListCount()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var result = service.ListCount();
            Assert.True(result.GetDataValue() > 0);
        }

        [Fact, Priority(5)]
        public void GetFilterCustomerListPaging()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            var result = service.List(paging);
            Assert.True(result.HasDataCount(3));
        }

        [Fact, Priority(6)]
        public void GetFilterCustomerListOrder()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var result = service.List(paging);
            Assert.True(result.HasDataCount(3));
        }

        [Fact, Priority(7)]
        public void GetFilterCustomerListOrderExpression()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var result = service.List(paging);
            Assert.True(result.HasDataCount(3));
        }

        [Fact, Priority(8)]
        public void GetFilterCustomerListPagingExpression()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var result = service.List(paging);
            Assert.True(result.HasDataCount(3));
        }

        [Fact, Priority(9)]
        public void GetFilterCustomerByName()
        {
            var service = serviceProvider.GetService<CustomerService>();
            var result = service.GetBy(x => x.Name == "Test 2");
            Assert.True(result.HasData());
        }
    }
}
