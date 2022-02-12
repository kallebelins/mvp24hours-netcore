//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.SQLServer.Test.Setup;
using Mvp24Hours.Application.SQLServer.Test.Support.Entities;
using Mvp24Hours.Application.SQLServer.Test.Support.Services.Async;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;
using Microsoft.Extensions.DependencyInjection;

namespace Mvp24Hours.Application.SQLServer.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test2QueryServiceAsync : IDisposable
    {
        private readonly StartupAsync startup;
        private readonly IServiceProvider serviceProvider;

        #region [ Ctor ]
        /// <summary>
        /// Initialize
        /// </summary>
        public Test2QueryServiceAsync()
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
        [Fact, Priority(1)]
        public async Task Get_Filter_Customer_List()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.ListAsync();
            // assert
            Assert.True(result.HasData());
        }
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_List_Any()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.ListAnyAsync();
            // assert
            Assert.True(result.GetDataValue());
        }
        [Fact, Priority(3)]
        public async Task Get_Filter_Customer_List_Count()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.ListCountAsync();
            // assert
            Assert.True(result.GetDataValue() > 0);
        }
        [Fact, Priority(4)]
        public async Task Get_Filter_Customer_List_Paging()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Asc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_List_Order_Desc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_List_Order_Asc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Order_Desc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(10)]
        public async Task Get_Filter_Customer_List_Paging_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(11)]
        public async Task Get_Filter_Customer_List_Navigation_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            // act
            var result = await service.ListAsync(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(12)]
        public async Task Get_Filter_Customer_GetById()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.GetByIdAsync(1);
            // assert
            Assert.True(result.GetDataFirstOrDefault() != null);
        }
        [Fact, Priority(13)]
        public async Task Get_Filter_Customer_GetById_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0, navigation: new List<string> { "Contacts" });
            // act
            var result = await service.GetByIdAsync(1, paging);
            // assert
            Assert.True(result.GetDataFirstOrDefault().Contacts.AnyOrNotNull());
        }
        [Fact, Priority(14)]
        public async Task Get_Filter_Customer_GetBy()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"));
            // assert
            Assert.True(result.HasData());
        }
        [Fact, Priority(15)]
        public async Task Get_Filter_Customer_GetBy_Any()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.GetByAnyAsync(x => x.Name.Contains("Test"));
            // assert
            Assert.True(result.GetDataValue());
        }
        [Fact, Priority(16)]
        public async Task Get_Filter_Customer_GetBy_Count()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            // act
            var result = await service.GetByCountAsync(x => x.Name.Contains("Test"));
            // assert
            Assert.True(result.GetDataValue() > 0);
        }
        [Fact, Priority(17)]
        public async Task Get_Filter_Customer_GetBy_Paging()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(18)]
        public async Task Get_Filter_Customer_GetBy_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(19)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(20)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(21)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(22)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(23)]
        public async Task Get_Filter_Customer_GetBy_Paging_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(24)]
        public async Task Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            // act
            var result = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        #endregion
    }
}
