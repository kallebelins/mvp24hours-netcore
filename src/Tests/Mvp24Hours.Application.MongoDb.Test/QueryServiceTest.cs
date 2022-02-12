//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Bson;
using Mvp24Hours.Application.MongoDb.Test.Support.Entities;
using Mvp24Hours.Application.MongoDb.Test.Support.Helpers;
using Mvp24Hours.Application.MongoDb.Test.Support.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
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
        public QueryServiceTest()
        {
            StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void Create_Many_Customers()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

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

            Assert.True(service.GetByCount(x => x.Active).GetDataValue() > 0);
        }

        [Fact, Priority(2)]
        public void Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.List().GetDataCount() > 0);
        }

        [Fact, Priority(3)]
        public void Get_Filter_Customer_List_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.ListAny().GetDataValue());
        }

        [Fact, Priority(4)]
        public void Get_Filter_Customer_List_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.ListCount().GetDataValue() > 0);
        }

        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            Assert.True(service.List(paging).HasDataCount(3));
        }

        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            Assert.True(service.List(paging).HasDataCount(3));
        }

        [Fact, Priority(7)]
        public void Get_Filter_Customer_List_Order_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            Assert.True(service.List(paging).HasDataCount(3));
        }

        [Fact, Priority(8)]
        public void Get_Filter_Customer_List_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            Assert.True(service.List(paging).HasDataCount(3));
        }

        [Fact, Priority(9)]
        public void Get_Filter_Customer_By_Name()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.GetBy(x => x.Name == "Test 2").HasData());
        }
    }
}
