using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Helpers;
using Mvp24Hours.Patterns.Test.Support.Helpers;
using System;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Patterns.Test
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class Test4Logging
    {
        public Test4Logging()
        {
            StartupHelper.ConfigureLogging();
        }

        [Fact, Priority(1)]
        public void Execute_Log()
        {
            var logging = ServiceProviderHelper.GetService<ILoggingService>();
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                logging.Error(ex);
            }
        }
    }
}
