//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.MySql.Test.Setup;
using Mvp24Hours.Application.MySql.Test.Support.Entities;
using Mvp24Hours.Application.MySql.Test.Support.Services;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.MySql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class Test1QueryService : IDisposable
    {
        private readonly Startup startup;
        private readonly IServiceProvider serviceProvider;

        #region [ Ctor ]
        /// <summary>
        /// Initialize
        /// </summary>
        public Test1QueryService()
        {
            startup = new Startup();
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
        public void Get_Filter_Customer_List()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.List();
            // assert
            Assert.True(result.HasData());
        }
        [Fact, Priority(2)]
        public void Get_Filter_Customer_List_Any()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.ListAny();
            // assert
            Assert.True(result.GetDataValue());
        }
        [Fact, Priority(3)]
        public void Get_Filter_Customer_List_Count()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.ListCount();
            // assert
            Assert.True(result.GetDataValue() > 0);
        }
        [Fact, Priority(4)]
        public void Get_Filter_Customer_List_Paging()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(5)]
        public void Get_Filter_Customer_List_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(6)]
        public void Get_Filter_Customer_List_Order_Asc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(7)]
        public void Get_Filter_Customer_List_Order_Desc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(8)]
        public void Get_Filter_Customer_List_Order_Asc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(9)]
        public void Get_Filter_Customer_List_Order_Desc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(10)]
        public void Get_Filter_Customer_List_Paging_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(11)]
        public void Get_Filter_Customer_List_Navigation_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            // act
            var result = service.List(paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(12)]
        public void Get_Filter_Customer_GetById()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.GetById(1);
            // assert
            Assert.True(result.GetDataValue() != null);
        }
        [Fact, Priority(13)]
        public void Get_Filter_Customer_GetById_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(1, 0, navigation: new List<string> { "Contacts" });
            // act
            var result = service.GetById(1, paging);
            // assert
            Assert.True(result.GetDataValue().Contacts.AnyOrNotNull());
        }
        [Fact, Priority(14)]
        public void Get_Filter_Customer_GetBy()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"));
            // assert
            Assert.True(result.HasData());
        }
        [Fact, Priority(15)]
        public void Get_Filter_Customer_GetBy_Any()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.GetByAny(x => x.Name.Contains("Test"));
            // assert
            Assert.True(result.GetDataValue());
        }
        [Fact, Priority(16)]
        public void Get_Filter_Customer_GetBy_Count()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            // act
            var result = service.GetByCount(x => x.Name.Contains("Test"));
            // assert
            Assert.True(result.GetDataValue() > 0);
        }
        [Fact, Priority(17)]
        public void Get_Filter_Customer_GetBy_Paging()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0);
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(18)]
        public void Get_Filter_Customer_GetBy_Navigation()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(19)]
        public void Get_Filter_Customer_GetBy_Order_Asc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, orderBy: new List<string> { "Name" });
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(20)]
        public void Get_Filter_Customer_GetBy_Order_Desc()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteria(3, 0, orderBy: new List<string> { "Name desc" });
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(21)]
        public void Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(22)]
        public void Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(23)]
        public void Get_Filter_Customer_GetBy_Paging_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }
        [Fact, Priority(24)]
        public void Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            // arrange
            var service = serviceProvider.GetService<CustomerService>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            // act
            var result = service.GetBy(x => x.Name.Contains("Test"), paging);
            // assert
            Assert.True(result.HasDataCount(3));
        }

        #endregion
    }
}
