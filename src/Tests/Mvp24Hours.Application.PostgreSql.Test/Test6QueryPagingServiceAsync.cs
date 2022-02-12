//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.PostgreSql.Test.Setup;
using Mvp24Hours.Application.PostgreSql.Test.Support.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Support.Services.Async;
using Mvp24Hours.Core.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.PostgreSql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test6QueryPagingServiceAsync : IDisposable
    {
        private readonly StartupAsync startup;
        private readonly IServiceProvider serviceProvider;

        #region [ Ctor ]
        /// <summary>
        /// Initialize
        /// </summary>
        public Test6QueryPagingServiceAsync()
        {
            startup = new StartupAsync();
            serviceProvider = startup.Initialize();
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            startup.Cleanup(serviceProvider);
        }
        #endregion

        #region [ List ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_List()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            // act
            var pagingResult = await service.ListWithPaginationAsync();
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Paging()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Asc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Desc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_List_Order_Asc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_List_Order_Desc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Paging_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Navigation_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            // act
            var pagingResult = await service.ListWithPaginationAsync(paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetBy()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"));
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Paging()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Paging_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerPagingServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            // act
            var pagingResult = await service.GetByWithPaginationAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(pagingResult.Paging != null);
        }
        #endregion
    }
}
