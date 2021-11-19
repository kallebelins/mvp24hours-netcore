//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Pipe.Test.Support.Helpers;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Pipe.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class PipelineAsyncTest
    {
        public PipelineAsyncTest()
        {
            StartupHelper.ConfigureServicesAsync();
        }

        [Fact, Priority(1)]
        public async Task Pipeline_Started()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();
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
            await pipeline.ExecuteAsync();
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(2)]
        public async Task Pipeline_Message_Content_Get()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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

            await pipeline.ExecuteAsync(message);
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(3)]
        public async Task Pipeline_Message_Content_Add()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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

            await pipeline.ExecuteAsync(message);

            // get content from result
            foreach (var item in pipeline.GetMessage().GetContentAll())
            {
                if (item is string)
                {
                    Trace.WriteLine(item);
                }
            }

            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(4)]
        public async Task Pipeline_Message_Content_Validate()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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

            var result1 = (await pipeline.ExecuteAsync()).GetMessage();
            var result2 = (await pipeline.ExecuteAsync("Parameter received.".ToMessage())).GetMessage();

            Assert.True(result1 != null && result2 != null);
        }

        [Fact, Priority(5)]
        public async Task Pipeline_Operation_Lock()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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

            await pipeline.ExecuteAsync("Parameter received.".ToMessage());
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(5)]
        public async Task Pipeline_Operation_Lock_Execute_Force()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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

            // interceptors -> locked-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Locked-Operation, only one time.");
            }, PipelineInterceptorType.Locked);

            await pipeline.ExecuteAsync("Parameter received.".ToMessage());
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(6)]
        public async Task Pipeline_Operation_Failure()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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
                Trace.WriteLine($"Failure....");
                input.SetFailure();
            });
            pipeline.Add(input =>
            {
                string param = input.GetContent<string>();
                Trace.WriteLine($"Test 3 - {param}");
            });

            // interceptors -> locked-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Faulty-Operation, only one time.");
            }, PipelineInterceptorType.Faulty);

            await pipeline.ExecuteAsync("Parameter received.".ToMessage());
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(7)]
        public async Task Pipeline_Operation_Lock_With_Notification()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

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

            // interceptors -> locked-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Locked-Operation, only one time.");
            }, PipelineInterceptorType.Locked);

            await pipeline.ExecuteAsync();

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
        public async Task Pipeline_Interceptors()
        {
            var pipeline = ServiceProviderHelper.GetService<IPipelineAsync>();

            // operations
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 1");
            });
            pipeline.Add(input =>
            {
                Trace.WriteLine("Test 2");
                Trace.WriteLine("Adding value to conditional interceptor test...");
                input.AddContent(1);
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 3");
            });

            // interceptors -> first-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("First-Operation, only one time.");
            }, PipelineInterceptorType.FirstOperation);

            // interceptors -> pre-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Pre-Operation");
            }, PipelineInterceptorType.PreOperation);

            // interceptors -> post-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Post-Operation");
            }, PipelineInterceptorType.PostOperation);

            // interceptors -> last-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Last-Operation, only one time.");
            }, PipelineInterceptorType.LastOperation);

            // interceptors -> locked-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Locked-Operation, only one time.");
            }, PipelineInterceptorType.Locked);

            // interceptors -> faulty-operation
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Faulty-Operation, only one time.");
            }, PipelineInterceptorType.Faulty);

            // interceptors -> conditional
            pipeline.AddInterceptors(_ =>
            {
                Trace.WriteLine("Conditional-Operation.");
            },
            input =>
            {
                return input.HasContent<int>();
            });

            await pipeline.ExecuteAsync();
            Assert.True(pipeline.GetMessage() != null);
        }
    }
}
