using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
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
