//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.PostgreSql.Test.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Helpers;
using Mvp24Hours.Application.PostgreSql.Test.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.PostgreSql.Test
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

        #region [ List ]
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
        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_List_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_List_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Pagging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var customers = service.List(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public void Get_Filter_Customer_GetById()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(1, 0);
            var customer = service.GetBy(x => x.Name.Contains("Test"), paging)?.FirstOrDefault();
            customer = service.GetById(customer?.Id);
            Assert.True(customer != null);
        }
        [Fact, Priority(2)]
        public void Get_Filter_Customer_GetBy()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customers = service.GetBy(x => x.Name.Contains("Test"));
            Assert.True(customers != null && customers.Count > 0);
        }
        [Fact, Priority(3)]
        public void Get_Filter_Customer_GetBy_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            bool any = service.GetByAny(x => x.Name.Contains("Test"));
            Assert.True(any);
        }
        [Fact, Priority(4)]
        public void Get_Filter_Customer_GetBy_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            int count = service.GetByCount(x => x.Name.Contains("Test"));
            Assert.True(count > 0);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_GetBy_Pagging()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_GetBy_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_GetBy_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_GetBy_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_GetBy_Pagging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var customers = service.GetBy(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        #endregion
    }
}
