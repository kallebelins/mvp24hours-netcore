using Mvp24Hours.Infrastructure.Logging;
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
            var logging = LoggingService.GetLoggingService();
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
