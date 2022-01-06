using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Patterns.Test.Support.Entities;
using Mvp24Hours.Patterns.Test.Support.Helpers;
using Mvp24Hours.Patterns.Test.Support.Services;
using System.Diagnostics;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test1Validation
    {

        [Fact, Priority(1)]
        public void Create_ComponentModel_Validation()
        {
            StartupHelper.ConfigureServices();

            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customer = new Customer
            {
                Active = true
            };
            service.Add(customer);

            var notfCtxOut = ServiceProviderHelper.GetService<INotificationContext>();
            if (notfCtxOut.HasErrorNotifications)
            {
                foreach (var item in notfCtxOut.Notifications)
                {
                    Trace.WriteLine(item.Message);
                }
            }

            Assert.True(notfCtxOut.HasErrorNotifications);
        }

        [Fact, Priority(1)]
        public void Create_Fluent_Validation()
        {
            StartupHelper.ConfigureServices(true);

            var service = ServiceProviderHelper.GetService<CustomerService>();
            var customer = new Customer
            {
                Active = true
            };
            service.Add(customer);

            var notfCtxOut = ServiceProviderHelper.GetService<INotificationContext>();
            if (notfCtxOut.HasErrorNotifications)
            {
                foreach (var item in notfCtxOut.Notifications)
                {
                    Trace.WriteLine(item.Message);
                }
            }

            Assert.True(notfCtxOut.HasErrorNotifications);
        }
    }
}
