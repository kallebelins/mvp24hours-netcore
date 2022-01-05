using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Patterns.Test.Support.Helpers;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test3Notification
    {
        public Test3Notification()
        {
            StartupHelper.ConfigureServices();
            StartupHelper.LoadData();
        }

        [Fact, Priority(1)]
        public void Get_Notification()
        {
            var notify1 = ServiceProviderHelper.GetService<INotificationContext>();
            notify1.Add("Test", "Message", Core.Enums.MessageType.Error);
            notify1.AddIfTrue(1 == 1, "Test", "Message", Core.Enums.MessageType.Error);

            var notify2 = ServiceProviderHelper.GetService<INotificationContext>();
            Assert.True(notify2.HasErrorNotifications);
        }
    }
}
