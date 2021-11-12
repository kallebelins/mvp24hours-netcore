//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Pipe.Test.Helpers;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Diagnostics;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Pipe.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class PipelineTest
    {
        public PipelineTest()
        {
            StartupHelper.ConfigureServices();
        }

        [Fact, Priority(1)]
        public void Pipeline_Started()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 1");
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 2");
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 3");
            });
            var result = pipeline.Execute();
            Assert.True(result != null);
        }

        [Fact, Priority(2)]
        public void Pipeline_Message_Content_Get()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 1 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 2 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 3 - {param}");
            });

            // define param
            var message = "Parameter received.".ToMessage();

            var result = pipeline.Execute(message);
            Assert.True(result != null);
        }

        [Fact, Priority(3)]
        public void Pipeline_Message_Content_Add()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                input.AddContent("teste1", $"Test 1 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                input.AddContent("teste2", $"Test 2 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                input.AddContent("teste3", $"Test 3 - {param}");
            });

            // define attachment for message 
            var message = "Parameter received.".ToMessage();

            var result = pipeline.Execute(message);

            // get content from result
            foreach (var item in result.GetContentAll())
            {
                if (item is string)
                {
                    Trace.WriteLine(item);
                }
            }

            Assert.True(result != null);
        }

        [Fact, Priority(4)]
        public void Pipeline_Message_Content_Validate()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                if (input.HasContent<string>())
                {
                    string param = input.GetContent<string>();
                    Trace.WriteLine($"Content - {param}");
                }
                else
                {
                    Trace.WriteLine("Content not found");
                }
            });

            var result1 = pipeline.Execute();
            var result2 = pipeline.Execute("Parameter received.".ToMessage());

            Assert.True(result1 != null && result2 != null);
        }

        [Fact, Priority(5)]
        public void Pipeline_Operation_Lock()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 1 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 2 - {param}");
                Trace.WriteLine($"Locking....");
                input.SetLock();
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 3 - {param}");
            });

            var result = pipeline.Execute("Parameter received.".ToMessage());
            Assert.True(result != null);
        }

        [Fact, Priority(5)]
        public void Pipeline_Operation_Lock_Execute_Force()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 1 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 2 - {param}");
                Trace.WriteLine($"Locking....");
                input.SetLock();
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 3 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Required - {param}");
            }, true);
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 4 - {param}");
            });

            var result = pipeline.Execute("Parameter received.".ToMessage());
            Assert.True(result != null);
        }

        [Fact, Priority(6)]
        public void Pipeline_Operation_Failure()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 1 - {param}");
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 2 - {param}");
                Trace.WriteLine($"Locking....");
                input.SetFailure();
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 3 - {param}");
            });

            var result = pipeline.Execute("Parameter received.".ToMessage());
            Assert.True(result != null);
        }

        [Fact, Priority(7)]
        public void Pipeline_Operation_Lock_With_Notification()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // add operations
            pipeline.Add(input =>
            {
                var notfCtxIn = ServiceProviderHelper.GetService<INotificationContext>();
                notfCtxIn.AddIfTrue(!input.HasContent<string>(), "Content not found", Core.Enums.MessageType.Error);
            });
            pipeline.Add(input =>
            {
                Trace.WriteLine("Operation blocked by 'Error' notification.");
            });

            pipeline.Execute();

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

        [Fact, Priority(8)]
        public void Pipeline_Interceptors()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipeline>();

            // operations
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 1");
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 2");
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 3");
            });

            // interceptors -> pre-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Pre-Operation");
            });

            // interceptors -> post-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Post-Operation");
            }, true);

            var result = pipeline.Execute();
            Assert.True(result != null);
        }

    }
}
