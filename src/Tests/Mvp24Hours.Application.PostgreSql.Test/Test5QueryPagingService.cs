//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.PostgreSql.Test.Support.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Support.Helpers;
using Mvp24Hours.Application.PostgreSql.Test.Support.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Helpers;
using System.Collections.Generic;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.PostgreSql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test5QueryPagingService
    {
        public Test5QueryPagingService()
        {
            StartupHelper.ConfigureServices();
            StartupHelper.LoadData();
        }

        #region [ List ]
        [Fact, Priority(2)]
        public void Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var pagingResult = service.ListWithPagination();
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0);
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_List_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_List_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var pagingResult = service.ListWithPagination(paging);
            Assert.True(pagingResult.Paging != null);
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public void Get_Filter_Customer_GetBy()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"));
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_GetBy_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0);
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_GetBy_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_GetBy_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_GetBy_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_GetBy_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var pagingResult = service.GetByWithPagination(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        #endregion
    }
}

