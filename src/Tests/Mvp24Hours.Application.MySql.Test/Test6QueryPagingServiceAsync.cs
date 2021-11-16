//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.MySql.Test.Entities;
using Mvp24Hours.Application.MySql.Test.Helpers;
using Mvp24Hours.Application.MySql.Test.Services.Async;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MySql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test6QueryPagingServiceAsync
    {
        public Test6QueryPagingServiceAsync()
        {
            StartupHelper.ConfigureServicesAsync();
            StartupHelper.LoadDataAsync();
        }

        #region [ List ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var pagingResult = await service.ListWithPaginationAsync();
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_List_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_List_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var pagingResult = await service.ListWithPaginationAsync(paging);
            Assert.True(pagingResult.Paging != null);
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetBy()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"));
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(pagingResult.Paging != null);
        }
        #endregion
    }
}
