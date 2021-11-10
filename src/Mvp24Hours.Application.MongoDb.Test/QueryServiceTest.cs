//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Bson;
using Mvp24Hours.Application.MongoDb.Test.Entities;
using Mvp24Hours.Application.MongoDb.Test.Helpers;
using Mvp24Hours.Application.MongoDb.Test.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MongoDb.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
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

            int count = service.GetByCount(x => x.Active);

            Assert.True(count > 0);
        }

        [Fact, Priority(2)]
        public void Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customers = service.List();
            Assert.True(customers != null && customers.Count > 0);
        }

        [Fact, Priority(3)]
        public void Get_Filter_Customer_List_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            bool any = service.ListAny();
            Assert.True(any);
        }

        [Fact, Priority(4)]
        public void Get_Filter_Customer_List_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            int count = service.ListCount();
            Assert.True(count > 0);
        }

        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Pagging()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }

        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }

        [Fact, Priority(7)]
        public void Get_Filter_Customer_List_Order_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }

        [Fact, Priority(8)]
        public void Get_Filter_Customer_List_Pagging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }

        [Fact, Priority(9)]
        public void Get_Filter_Customer_By_Name()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customer = service.GetBy(x => x.Name == "Test 2");
            Assert.True(customer != null);
        }
    }
}
