//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Application.Pipe.Test.Operations;
using Mvp24Hours.Application.Pipe.Test.Rollbacks;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Extensions;
using Mvp24Hours.Infrastructure.Pipe;
using System;
using System.Diagnostics;
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
        [Fact, Priority(1)]
        public async Task PipelineStarted()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineMessageContentGet()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineMessageContentAdd()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineMessageContentValidate()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineOperationLock()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineOperationLockExecuteForce()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineOperationFailure()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineOperationLockWithNotification()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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

            await pipeline.ExecuteAsync();

            var message = pipeline.GetMessage();

            foreach (var item in message.Messages)
            {
                Trace.WriteLine(item.Message);
            }

            // assert
            Assert.True(message.IsFaulty);
        }

        [Fact, Priority(8)]
        public async Task PipelineInterceptors()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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
        public async Task PipelineEventInterceptors()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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

            await pipeline.ExecuteAsync();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(10)]
        public async Task PipelineEventInterceptorsWithLock()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

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

            await pipeline.ExecuteAsync();

            // assert
            Assert.True(pipeline.GetMessage() != null);
        }

        [Fact, Priority(11)]
        public void PipelineWithOperation()
        {
            // arrange
            IPipelineAsync pipeline = new PipelineAsync();

            // act
            pipeline.Add<OperationTestAsync>();

            // operations
            pipeline.ExecuteAsync();
            var result = pipeline.GetMessage().GetContent<int>("key-test");

            // assert
            Assert.True(result == 1);
        }

        [Fact, Priority(12)]
        public async Task PipelineWithRollbackOperations()
        {
            // arrange
            var pipeline = new PipelineAsync() { ForceRollbackOnFalure = true };
            RollbackTestContext.Results.Clear();

            // act
            pipeline.Add<RollbackOperationTestAsyncStep1>();
            pipeline.Add<RollbackOperationTestAsyncStep2>();
            pipeline.Add<RollbackOperationTestAsyncStep3>();

            // operations
            await pipeline.ExecuteAsync();
            var resultExecutionStep1 = pipeline.GetMessage().GetContent<int>("key-test-step1");
            var resultExecutionStep2 = pipeline.GetMessage().GetContent<int>("key-test-step2");
            var resultExecutionStep3 = pipeline.GetMessage().GetContent<int>("key-test-step3");
            var resultRollbackStep1 = pipeline.GetMessage().GetContent<int>("key-test-rollback-step1");
            var resultRollbackStep2 = pipeline.GetMessage().GetContent<int>("key-test-rollback-step2");
            var resultRollbackStep3 = pipeline.GetMessage().HasContent("key-test-rollback-step3");

            var resultIndexStep1 = RollbackTestContext.Results.IndexOf("key-test-rollback-step1");
            var resultIndexStep2 = RollbackTestContext.Results.IndexOf("key-test-rollback-step2");

            // assert
            Assert.Equal(1, resultExecutionStep1);
            Assert.Equal(10, resultRollbackStep1);
            Assert.Equal(2, resultExecutionStep2);
            Assert.Equal(20, resultRollbackStep2);
            Assert.Equal(3, resultExecutionStep3);
            Assert.False(resultRollbackStep3);

            Assert.Equal(1, resultIndexStep1); //Step 1 must be after step 2 because all rollbacks are executed top-down
            Assert.Equal(0, resultIndexStep2);
        }

        [Fact, Priority(13)]
        public async Task PipelineWithRollbackOperationsWithoutForceRollbackOnFalure()
        {
            // arrange
            var pipeline = new PipelineAsync() { ForceRollbackOnFalure = false };

            // act
            pipeline.Add<RollbackOperationTestAsyncStep1>();
            pipeline.Add<RollbackOperationTestAsyncStep2>();
            pipeline.Add<RollbackOperationTestAsyncStep3>();

            // operations
            await pipeline.ExecuteAsync();
            var resultExecutionStep1 = pipeline.GetMessage().GetContent<int>("key-test-step1");
            var resultExecutionStep2 = pipeline.GetMessage().GetContent<int>("key-test-step2");
            var resultExecutionStep3 = pipeline.GetMessage().GetContent<int>("key-test-step3");
            var resultRollbackStep1 = pipeline.GetMessage().HasContent("key-test-rollback-step1");
            var resultRollbackStep2 = pipeline.GetMessage().HasContent("key-test-rollback-step2");
            var resultRollbackStep3 = pipeline.GetMessage().HasContent("key-test-rollback-step3");

            // assert
            Assert.Equal(1, resultExecutionStep1);
            Assert.Equal(2, resultExecutionStep2);
            Assert.Equal(3, resultExecutionStep3);
            Assert.False(resultRollbackStep1);
            Assert.False(resultRollbackStep2);
            Assert.False(resultRollbackStep3);
        }

        [Fact, Priority(14)]
        public async Task PipelineWithWithAllowPropagateException()
        {
            // arrange
            var pipeline = new PipelineAsync() { AllowPropagateException = true };
            var exception = default(Exception);

            // act
            pipeline.Add<RollbackOperationTestAsyncStep1>();
            pipeline.Add<RollbackOperationTestAsyncStep2>();
            pipeline.Add<RollbackOperationTestAsyncStep3>();

            try
            {
                // operations
                await pipeline.ExecuteAsync();
            }
            catch(Exception ex)
            {
                exception = ex;
            }   

            // assert
            Assert.NotNull(exception);
            Assert.Equal("My Exception 123", exception.Message);
            Assert.Equal(typeof(InvalidOperationException), exception.GetType());
        }

        [Fact, Priority(15)]
        public async Task PipelineWithWithoutAllowPropagateException()
        {
            // arrange
            var pipeline = new PipelineAsync(); //AllowPropagateException = false
            var exception = default(Exception);

            // act
            pipeline.Add<RollbackOperationTestAsyncStep1>();
            pipeline.Add<RollbackOperationTestAsyncStep2>();
            pipeline.Add<RollbackOperationTestAsyncStep3>();

            try
            {
                // operations
                await pipeline.ExecuteAsync();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // assert
            Assert.Null(exception);
        }
    }
}
