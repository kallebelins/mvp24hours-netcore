//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.PostgreSql.Test.Support.Entities;
using Mvp24Hours.Application.PostgreSql.Test.Support.Helpers;
using Mvp24Hours.Application.PostgreSql.Test.Support.Services.Async;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.PostgreSql.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test2QueryServiceAsync
    {
        public Test2QueryServiceAsync()
        {
            StartupHelper.ConfigureServicesAsync();
            StartupHelper.LoadDataAsync();
        }

        #region [ List ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_List()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            Assert.True(await service.ListAsync().HasDataAsync());
        }
        [Fact, Priority(3)]
        public async Task Get_Filter_Customer_List_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            Assert.True(await service.ListAnyAsync().GetDataValueAsync());
        }
        [Fact, Priority(4)]
        public async Task Get_Filter_Customer_List_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            Assert.True(await service.ListCountAsync().GetDataValueAsync() > 0);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_List_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_List_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_List_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_List_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_List_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            Assert.True(await service.ListAsync(paging).HasDataCountAsync(3));
        }
        #endregion

        #region [ GetBy ]
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetById()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0);
            var customer = await service.GetByAsync(x => x.Name.Contains("Test"), paging)
                .GetDataFirstOrDefaultAsync() as Customer;
            customer = await service.GetByIdAsync(customer?.Id)
                .GetDataFirstOrDefaultAsync() as Customer;
            Assert.True(customer != null);
        }
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetById_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(1, 0, navigation: new List<string> { "Contacts" });
            var customer = (await service.GetByAsync(x => x.Contacts.Any(), paging))
                .GetDataFirstOrDefault() as Customer;
            customer = await service.GetByIdAsync(customer?.Id, paging)
                .GetDataFirstOrDefaultAsync() as Customer;
            Assert.True(customer.Contacts.AnyOrNotNull());
        }
        [Fact, Priority(2)]
        public async Task Get_Filter_Customer_GetBy()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test")).HasDataAsync());
        }
        [Fact, Priority(3)]
        public async Task Get_Filter_Customer_GetBy_Any()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            Assert.True(await service.GetByAnyAsync(x => x.Name.Contains("Test")).GetDataValueAsync());
        }
        [Fact, Priority(4)]
        public async Task Get_Filter_Customer_GetBy_Count()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            Assert.True(await service.GetByCountAsync(x => x.Name.Contains("Test")).GetDataValueAsync() > 0);
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Paging()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0);
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(5)]
        public async Task Get_Filter_Customer_GetBy_Navigation()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, navigation: new List<string> { "Contacts" });
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name" });
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(6)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteria(3, 0, new List<string> { "Name desc" });
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(7)]
        public async Task Get_Filter_Customer_GetBy_Order_Asc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByAscendingExpr.Add(x => x.Name);
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(8)]
        public async Task Get_Filter_Customer_GetBy_Order_Desc_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.OrderByDescendingExpr.Add(x => x.Name);
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Paging_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        [Fact, Priority(9)]
        public async Task Get_Filter_Customer_GetBy_Navigation_Expression()
        {
            var service = ServiceProviderHelper.GetService<CustomerServiceAsync>();
            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);
            Assert.True(await service.GetByAsync(x => x.Name.Contains("Test"), paging).HasDataCountAsync(3));
        }
        #endregion
    }
}
