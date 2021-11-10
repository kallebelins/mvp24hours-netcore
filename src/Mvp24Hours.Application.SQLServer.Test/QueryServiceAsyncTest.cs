//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.SQLServer.Test.Entities;
using Mvp24Hours.Application.SQLServer.Test.Helpers;
using Mvp24Hours.Application.SQLServer.Test.Services;
using Mvp24Hours.Application.SQLServer.Test.Services.Async;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.SQLServer.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class QueryServiceAsyncTest
    {
        public QueryServiceAsyncTest()
        {
            StartupHelper.ConfigureServicesAsync();
        }

        #region [ List ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var customers = await service.ListAsync();
            Assert.True(customers != null && customers.Count > 0);
        }
        [Fact, Priority(3)]
        public async Task Get_Filter_Customer_List_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            bool any = await service.ListAnyAsync();
            Assert.True(any);
        }
        [Fact, Priority(4)]
        public async Task Get_Filter_Customer_List_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            int count = await service.ListCountAsync();
            Assert.True(count > 0);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Pagging()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_List_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_List_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Pagging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var customers = await service.ListAsync(paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetById()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0);
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            var customer = customers?.FirstOrDefault();
            customer = await service.GetByIdAsync(customer?.Id);
            Assert.True(customer != null);
        }
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetBy()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"));
            Assert.True(customers != null && customers.Count > 0);
        }
        [Fact, Priority(3)]
        public async Task Get_Filter_Customer_GetBy_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            bool any = await service.GetByAnyAsync(x => x.Name.Contains("Test"));
            Assert.True(any);
        }
        [Fact, Priority(4)]
        public async Task Get_Filter_Customer_GetBy_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            int count = await service.GetByCountAsync(x => x.Name.Contains("Test"));
            Assert.True(count > 0);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Pagging()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Pagging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            var customers =await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            var customers = await service.GetByAsync(x => x.Name.Contains("Test"), paging);
            Assert.True(customers != null && customers.Count == 3);
        }
        #endregion
    }
}
