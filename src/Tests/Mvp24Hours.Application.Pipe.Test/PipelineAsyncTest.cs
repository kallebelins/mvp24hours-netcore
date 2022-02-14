//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.Pipe.Test.Setup;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Extensions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Pipe.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class PipelineAsyncTest
    {
        private readonly StartupAsync startup;

        /// <summary>
        /// Initialize
        /// </summary>
        public PipelineAsyncTest()
        {
            startup = new StartupAsync();
        }

        [Fact, Priority(1)]
        public async Task Pipeline_Started()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act
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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(2)]
        public async Task Pipeline_Message_Content_Get()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(3)]
        public async Task Pipeline_Message_Content_Add()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(4)]
        public async Task Pipeline_Message_Content_Validate()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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
            await pipeline.ExecuteAsync();
            var result1 = pipeline.GetMessage();
            await pipeline.ExecuteAsync("Parameter received.".ToMessage());
            var result2 = pipeline.GetMessage();

            // assert
            Assert.True(result1 != null && result2 != null);
        }

        [Fact, Priority(5)]
        public async Task Pipeline_Operation_Lock()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(5)]
        public async Task Pipeline_Operation_Lock_Execute_Force()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(6)]
        public async Task Pipeline_Operation_Failure()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(7)]
        public async Task Pipeline_Operation_Lock_With_Notification()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

            // add operations
            pipeline.Add(input =>
            {
                var notfCtxIn = serviceProvider.GetService<INotificationContext>();
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

            var notfCtxOut = serviceProvider.GetService<INotificationContext>();
            if (notfCtxOut.HasErrorNotifications)
            {
                foreach (var item in notfCtxOut.Notifications)
                {
                    Trace.WriteLine(item.Message);
                }
            }

            // assert
            Assert.True(notfCtxOut.HasErrorNotifications);
        }

        [Fact, Priority(8)]
        public async Task Pipeline_Interceptors()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(9)]
        public async Task Pipeline_Event_Interceptors()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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

            // event interceptors -> first-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("First-Operation, event.");
            }, PipelineInterceptorType.FirstOperation);

            // event interceptors -> pre-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Pre-Operation, event.");
            }, PipelineInterceptorType.PreOperation);

            // event interceptors -> post-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Post-Operation, event.");
            }, PipelineInterceptorType.PostOperation);

            // event interceptors -> last-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Last-Operation, event.");
            }, PipelineInterceptorType.LastOperation);

            // event interceptors -> locked-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Locked-Operation, event.");
            }, PipelineInterceptorType.Locked);

            // event interceptors -> faulty-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Faulty-Operation, event.");
            }, PipelineInterceptorType.Faulty);

            // event interceptors -> conditional
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Conditional-Operation, event.");
            },
            input =>
            {
                return input.HasContent<int>();
            });

            await pipeline.ExecuteAsync();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(10)]
        public async Task Pipeline_Event_Interceptors_With_Lock()
        {
            // arrange
            var serviceProvider = startup.Initialize();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act

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
            pipeline.Add(input =>
            {
                Trace.WriteLine("Test 3");
                Trace.WriteLine("Locking...");
                input.SetLock();
            });

            // event interceptors -> first-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("First-Operation, event.");
            }, PipelineInterceptorType.FirstOperation);

            // event interceptors -> pre-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Pre-Operation, event.");
            }, PipelineInterceptorType.PreOperation);

            // event interceptors -> post-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Post-Operation, event.");
            }, PipelineInterceptorType.PostOperation);

            // event interceptors -> last-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Last-Operation, event.");
            }, PipelineInterceptorType.LastOperation);

            // event interceptors -> locked-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Locked-Operation, event.");
            }, PipelineInterceptorType.Locked);

            // event interceptors -> faulty-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Faulty-Operation, event.");
            }, PipelineInterceptorType.Faulty);

            // event interceptors -> conditional
            pipeline.AddInterceptors((input, e) =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine("Conditional-Operation, event.");
            },
            input =>
            {
                return input.HasContent<int>();
            });

            await pipeline.ExecuteAsync();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(11)]
        public async Task Pipeline_Factory()
        {
            // arrange
            var serviceProvider = startup.InitializeWithFactory();
            var pipeline = serviceProvider.GetService<IPipelineAsync>();

            // act
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 1");
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 2");
            });
            await pipeline.ExecuteAsync();

            // assert
            var message = pipeline.GetMessage();
            Assert.True(message.GetContent<int>("factory") == 1);
        }

    }
}
