//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Application.Pipe.Test.Operations;
using Mvp24Hours.Application.Pipe.Test.Setup;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Extensions;
using System.Diagnostics;
using Xunit;
using Xunit.Priority;

namespace Mvp24Hours.Application.Pipe.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class PipelineWithInjectionTest
    {
        private readonly Startup startup;

        /// <summary>
        /// Initialize
        /// </summary>
        public PipelineWithInjectionTest()
        {
            startup = new Startup();
        }

        [Fact, Priority(1)]
        public void PipelineStarted()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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
            pipeline.Execute();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(2)]
        public void PipelineMessageContentGet()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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

            pipeline.Execute(message);

            // assert
            Assert.NotNull(pipeline.GetMessage());
        }

        [Fact, Priority(3)]
        public void PipelineMessageContentAdd()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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

            pipeline.Execute(message);

            // get content from result
            foreach (var item in pipeline.GetMessage().GetContentAll())
            {
                if (item is string)
                {
                    Trace.WriteLine(item);
                }
            }

            // assert
            Assert.NotNull(pipeline.GetMessage());
        }

        [Fact, Priority(4)]
        public void PipelineMessageContentValidate()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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
            pipeline.Execute();
            var result1 = pipeline.GetMessage();
            pipeline.Execute("Parameter received.".ToMessage());
            var result2 = pipeline.GetMessage();

            // assert
            Assert.True(result1 != null && result2 != null);
        }

        [Fact, Priority(5)]
        public void PipelineOperationLock()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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

            pipeline.Execute("Parameter received.".ToMessage());

            // assert
            Assert.NotNull(pipeline.GetMessage());
        }

        [Fact, Priority(5)]
        public void PipelineOperationLockExecuteForce()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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

            pipeline.Execute("Parameter received.".ToMessage());

            // assert
            Assert.NotNull(pipeline.GetMessage());
        }

        [Fact, Priority(6)]
        public void PipelineOperationFailure()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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

            pipeline.Execute("Parameter received.".ToMessage());

            // assert
            Assert.NotNull(pipeline.GetMessage());
        }

        [Fact, Priority(7)]
        public void PipelineOperationLockWithNotification()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

            // act

            // add operations
            pipeline.Add(input =>
            {
                input.Messages.AddMessage("Content not found", Core.Enums.MessageType.Error);
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

            pipeline.Execute();

            var message = pipeline.GetMessage();

            foreach (var item in message.Messages)
            {
                Trace.WriteLine(item.Message);
            }

            // assert
            Assert.True(message.IsFaulty);
        }

        [Fact, Priority(8)]
        public void PipelineInterceptors()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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

            pipeline.Execute();

            // assert
            Assert.NotNull(pipeline.GetMessage());
        }

        [Fact, Priority(9)]
        public void PipelineEventInterceptors()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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
                Trace.WriteLine("First-Operation, event.");
            }, PipelineInterceptorType.FirstOperation);

            // event interceptors -> pre-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Pre-Operation, event.");
            }, PipelineInterceptorType.PreOperation);

            // event interceptors -> post-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Post-Operation, event.");
            }, PipelineInterceptorType.PostOperation);

            // event interceptors -> last-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Last-Operation, event.");
            }, PipelineInterceptorType.LastOperation);

            // event interceptors -> locked-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Locked-Operation, event.");
            }, PipelineInterceptorType.Locked);

            // event interceptors -> faulty-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Faulty-Operation, event.");
            }, PipelineInterceptorType.Faulty);

            // event interceptors -> conditional
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Conditional-Operation, event.");
            },
            input =>
            {
                return input.HasContent<int>();
            });

            pipeline.Execute();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(10)]
        public void PipelineEventInterceptorsWithLock()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

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
                Trace.WriteLine("First-Operation, event.");
            }, PipelineInterceptorType.FirstOperation);

            // event interceptors -> pre-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Pre-Operation, event.");
            }, PipelineInterceptorType.PreOperation);

            // event interceptors -> post-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Post-Operation, event.");
            }, PipelineInterceptorType.PostOperation);

            // event interceptors -> last-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Last-Operation, event.");
            }, PipelineInterceptorType.LastOperation);

            // event interceptors -> locked-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Locked-Operation, event.");
            }, PipelineInterceptorType.Locked);

            // event interceptors -> faulty-operation
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Faulty-Operation, event.");
            }, PipelineInterceptorType.Faulty);

            // event interceptors -> conditional
            pipeline.AddInterceptors((input, e) =>
            {
                Trace.WriteLine("Conditional-Operation, event.");
            },
            input =>
            {
                return input.HasContent<int>();
            });

            pipeline.Execute();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(11)]
        public void PipelineFactory()
        {
            // arrange
            var serviceProvider = startup.SetupInjectionFactory();
            var pipeline = serviceProvider.GetService<IPipeline>();

            // act
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 1");
            });
            pipeline.Add(_ =>
            {
                Trace.WriteLine("Test 2");
            });
            pipeline.Execute();

            // assert
            var message = pipeline.GetMessage();
            Assert.Equal(1, message.GetContent<int>("factory"));
        }

        [Fact, Priority(12)]
        public void PipelineWithOperation()
        {
            // arrange
            var serviceProvider = startup.SetupInjection();
            var pipeline = serviceProvider.GetService<IPipeline>();

            // act
            pipeline.Add<OperationTest>();

            // operations
            pipeline.Execute();
            var result = pipeline.GetMessage().GetContent<int>("key-test");

            // assert
            Assert.True(result == 1);
        }
    }
}
