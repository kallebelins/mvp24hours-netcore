using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Patterns.Test.Support.Entities;
using Mvp24Hours.Patterns.Test.Support.Helpers;
using Mvp24Hours.Patterns.Test.Support.Services;
using Mvp24Hours.Patterns.Test.Support.Specifications;
using System;
using System.Linq.Expressions;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test2Specification
    {
        public Test2Specification()
        {
            StartupHelper.ConfigureServices();
            StartupHelper.LoadData();
        }

        [Fact, Priority(1)]
        public void Get_Customer_ByEmail()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            Expression<Func<Customer, bool>> filter = x => x.Active;

            filter = filter.And<Customer, CustomerHasEmailContactSpec>();

            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);

            var boResult = service.GetBy(filter, paging);

            Assert.True(boResult.HasData());
        }

        [Fact, Priority(2)]
        public void Get_Customer_ByCell()
        {
            var service = ServiceProviderHelper.GetService<CustomerService>();

            Expression<Func<Customer, bool>> filter = x => x.Active;

            filter = filter.And<Customer, CustomerHasCellContactSpec>();

            var paging = new PagingCriteriaExpression<Customer>(3, 0);
            paging.NavigationExpr.Add(x => x.Contacts);

            var boResult = service.GetBy(filter, paging);

            Assert.True(boResult.HasData());
        }
    }
}
