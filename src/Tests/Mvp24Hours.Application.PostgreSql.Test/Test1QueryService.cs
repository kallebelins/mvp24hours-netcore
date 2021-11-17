//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.PostgreSql.Test.Support.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Support.Helpers;
using Mvp24Hours.Application.PostgreSql.Test.Support.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
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
    public class Test1QueryService
    {
        public Test1QueryService()
        {
            StartupHelper.ConfigureServices();
            StartupHelper.LoadData();
        }

        #region [ List ]
        [Fact, Priority(2)]
        public void Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.List().HasData());
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
        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            Assert.True(service.List(paging).HasDataCount(3));
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            Assert.True(service.List(paging).HasDataCount(3));
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            Assert.True(service.List(paging).HasDataCount(3));
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_List_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            Assert.True(service.List(paging).HasDataCount(3));
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_List_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            Assert.True(service.List(paging).HasDataCount(3));
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            Assert.True(service.List(paging).HasDataCount(3));
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            Assert.True(service.List(paging).HasDataCount(3));
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public void Get_Filter_Customer_GetById()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(1, 0);
            var customer = service.GetBy(x => x.Name.Contains("Test"), paging)
                .GetDataFirstOrDefault() as Customer;
            customer = service.GetById(customer?.Id)
                .GetDataFirstOrDefault() as Customer;
            Assert.True(customer != null);
        }
        [Fact, Priority(2)]
        public void Get_Filter_Customer_GetById_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(1, 0, navigation: new List<string> { "Contacts" });
            var customer = service.GetBy(x => x.Contacts.Any(), paging)
                .GetDataFirstOrDefault() as Customer;
            customer = service.GetById(customer?.Id, paging)
                .GetDataFirstOrDefault() as Customer;
            Assert.True(customer.Contacts.AnyOrNotNull());
        }
        [Fact, Priority(2)]
        public void Get_Filter_Customer_GetBy()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.GetBy(x => x.Name.Contains("Test")).HasData());
        }
        [Fact, Priority(3)]
        public void Get_Filter_Customer_GetBy_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.GetByAny(x => x.Name.Contains("Test")).GetDataValue());
        }
        [Fact, Priority(4)]
        public void Get_Filter_Customer_GetBy_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            Assert.True(service.GetByCount(x => x.Name.Contains("Test")).GetDataValue() > 0);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_GetBy_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_GetBy_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_GetBy_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_GetBy_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_GetBy_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            Assert.True(service.GetBy(x => x.Name.Contains("Test"), paging).HasDataCount(3));
        }
        #endregion
    }
}
