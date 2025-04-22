//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mvp24Hours.Core.Contract.Application.Pipe;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Pipe.Configuration;
using Mvp24Hours.Infrastructure.Pipe.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Pipe.IPipeline"/>
    /// </summary>
    public class Pipeline : PipelineBase, IPipeline
    {
        #region [ Ctor ]

        public Pipeline(IServiceProvider _provider = null)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipeline-ctor");
            this.provider = _provider;

            var options = _provider?.GetService<IOptions<PipelineOptions>>()?.Value;
            if (options != null)
            {
                this.IsBreakOnFail = options.IsBreakOnFail;
            }

            operations = new List<IOperation>();
            executedOperations = new List<IOperation>();

            preCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>>();
            postCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>>();
            dictionaryInterceptors = new Dictionary<PipelineInterceptorType, IList<IOperation>>();

            dictionaryEventInterceptors = new Dictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>>();
            preEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
            postEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
        }

        #endregion

        #region [ Fields / Properties ]
        private readonly IServiceProvider provider;
        private readonly IList<IOperation> operations;
        private readonly IList<IOperation> executedOperations;

        private readonly IDictionary<PipelineInterceptorType, IList<IOperation>> dictionaryInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>> preCustomInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>> postCustomInterceptors;

        private readonly IDictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>> dictionaryEventInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> preEventCustomInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> postEventCustomInterceptors;
        #endregion

        #region [ Methods ]

        #region [ Get ]
        public IList<IOperation> GetOperations() => operations;
        public IDictionary<PipelineInterceptorType, IList<IOperation>> GetInterceptors() => dictionaryInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>> GetPreInterceptors() => preCustomInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperation>> GetPostInterceptors() => postCustomInterceptors;
        public IDictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>> GetEvents() => dictionaryEventInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> GetPreEvents() => preEventCustomInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> GetPostEvents() => postEventCustomInterceptors;
        #endregion

        public IPipeline Add<T>() where T : class, IOperation
        {
            IOperation instance = provider?.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return Add(Activator.CreateInstance<T>());
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "Operation not found. Check if it has been registered in this context.");
                }
            }
            return Add(instance);
        }
        public IPipeline Add(IOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            this.operations.Add(operation);
            return this;
        }
        public IPipeline Add(Action<IPipelineMessage> action, bool isRequired = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            this.operations.Add(new OperationAction(action, isRequired));
            return this;
        }

        public IPipeline AddBuilder<T>() where T : class, IPipelineBuilder
        {
            IPipelineBuilder pipelineBuilder = provider?.GetService<T>();
            if (pipelineBuilder == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return Activator.CreateInstance<T>().Builder(this);
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "PipelineBuilder not found. Check if it has been registered in this context.");
                }
            }
            var result = pipelineBuilder.Builder(this);
            return result;
        }
        public IPipeline AddBuilder(IPipelineBuilder pipelineBuilder)
        {
            if (pipelineBuilder == null)
            {
                throw new ArgumentNullException(nameof(pipelineBuilder), "PipelineBuilder has not been defined or is null.");
            }
            pipelineBuilder.Builder(this);
            return this;
        }

        public IPipeline AddInterceptors<T>(PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation) where T : class, IOperation
        {
            IOperation instance = provider?.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return AddInterceptors(Activator.CreateInstance<T>(), pipelineInterceptor);
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, pipelineInterceptor);
        }
        public IPipeline AddInterceptors(IOperation operation, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            if (!this.dictionaryInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryInterceptors.Add(pipelineInterceptor, new List<IOperation>());
            }
            this.dictionaryInterceptors[pipelineInterceptor].Add(operation);
            return this;
        }
        public IPipeline AddInterceptors(Action<IPipelineMessage> action, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            if (!this.dictionaryInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryInterceptors.Add(pipelineInterceptor, new List<IOperation>());
            }
            this.dictionaryInterceptors[pipelineInterceptor].Add(new OperationAction(action));
            return this;
        }
        public IPipeline AddInterceptors<T>(Func<IPipelineMessage, bool> condition, bool postOperation = true) where T : class, IOperation
        {
            IOperation instance = provider?.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return AddInterceptors(Activator.CreateInstance<T>(), condition, postOperation);
                }
                else
                {
                    throw new ArgumentNullException(string.Empty, "Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, condition, postOperation);
        }
        public IPipeline AddInterceptors(IOperation operation, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            if (postOperation)
            {
                this.postCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, operation));
            }
            else
            {
                this.preCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, operation));
            }
            return this;
        }
        public IPipeline AddInterceptors(Action<IPipelineMessage> action, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "Condition is mandatory.");
            }
            if (postOperation)
            {
                this.postCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, new OperationAction(action)));
            }
            else
            {
                this.preCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperation>(condition, new OperationAction(action)));
            }
            return this;
        }
        public IPipeline AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler has not been defined or is null.");
            }

            if (!this.dictionaryEventInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryEventInterceptors.Add(pipelineInterceptor, new List<EventHandler<IPipelineMessage, EventArgs>>());
            }
            this.dictionaryEventInterceptors[pipelineInterceptor].Add(handler);
            return this;
        }
        public IPipeline AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler is mandatory.");
            }
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "Condition is mandatory.");
            }
            if (postOperation)
            {
                this.postEventCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>(condition, handler));
            }
            else
            {
                this.preEventCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>(condition, handler));
            }
            return this;
        }

        public void Execute(IPipelineMessage input = null)
        {
            executedOperations.Clear();
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-start");
            try
            {
                Message = input ?? Message;
                Message = RunOperations(this.operations, Message);
            }
            finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-end"); }
        }
        protected virtual IPipelineMessage RunOperations(IList<IOperation> _operations, IPipelineMessage input, bool onlyOperationDefault = false)
        {
            if (!_operations.AnySafe())
            {
                return input;
            }

            input ??= new PipelineMessage();

            if (!onlyOperationDefault)
            {
                RunEventInterceptors(input, PipelineInterceptorType.FirstOperation);
                RunOperationInterceptors(input, PipelineInterceptorType.FirstOperation);
            }

            _ = _operations.Aggregate(input, (current, operation) =>
              {
                  if ((current.IsFaulty) && this.IsBreakOnFail)
                  {
                      return current;
                  }

                  if (current.IsLocked && !operation.IsRequired)
                  {
                      return current;
                  }

                  try
                  {
                      // pre-operation
                      if (!onlyOperationDefault)
                      {
                          // events
                          RunCustomEventInterceptors(input, false);
                          RunEventInterceptors(input, PipelineInterceptorType.PreOperation);

                          // operations
                          RunCustomOperationInterceptors(input, false);
                          RunOperationInterceptors(input, PipelineInterceptorType.PreOperation);
                      }

                      // operation
                      TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-operation-start", $"operation:{operation.GetType().Name}");
                      try
                      {
                          operation.Execute(current);
                      }
                      finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-operation-end", $"operation:{operation.GetType().Name}"); }

                      // post-operation
                      if (!onlyOperationDefault)
                      {
                          // events
                          RunEventInterceptors(input, PipelineInterceptorType.PostOperation);
                          RunCustomEventInterceptors(input);

                          // operations
                          RunOperationInterceptors(current, PipelineInterceptorType.PostOperation);
                          RunCustomOperationInterceptors(current);

                          if (current.IsLocked)
                          {
                              RunEventInterceptors(input, PipelineInterceptorType.Locked, true);
                              RunOperationInterceptors(current, PipelineInterceptorType.Locked, true);
                          }

                          if (current.IsFaulty)
                          {
                              RunEventInterceptors(input, PipelineInterceptorType.Faulty, true);
                              RunOperationInterceptors(current, PipelineInterceptorType.Faulty, true);
                          }
                          else
                          {
                              executedOperations.Add(operation);
                          }
                      }

                      return current;
                  }
                  catch (Exception ex)
                  {
                      TelemetryHelper.Execute(TelemetryLevels.Error, "pipe-pipeline-execute-failure", ex);
                      current.Messages.Add(new MessageResult((ex?.InnerException ?? ex).Message, MessageType.Error));
                      input.AddContent(ex);
                  }
                  return current;
              });

            if (!onlyOperationDefault && (!input.IsFaulty))
            {
                RunEventInterceptors(input, PipelineInterceptorType.LastOperation);
                RunOperationInterceptors(input, PipelineInterceptorType.LastOperation);
            }

            if (!onlyOperationDefault && input.IsFaulty && ForceRollbackOnFalure)
            {
                RunRollbackOperations(input);
            }

            return input;
        }

        protected virtual void RunOperationInterceptors(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryInterceptors.ContainsKey(interceptorType))
            {
                RunOperations(dictionaryInterceptors[interceptorType], input, true);
                if (canClearList)
                {
                    dictionaryInterceptors.Remove(interceptorType);
                }
            }
        }
        protected virtual void RunCustomOperationInterceptors(IPipelineMessage input, bool postOperation = true)
        {
            if (postOperation)
            {
                if (postCustomInterceptors.AnySafe())
                {
                    foreach (var ci in postCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        RunOperations(new List<IOperation> { ci.Value }, input, true);
                    }
                }
            }
            else
            {
                if (preCustomInterceptors.AnySafe())
                {
                    foreach (var ci in preCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        RunOperations(new List<IOperation> { ci.Value }, input, true);
                    }
                }
            }
        }
        protected virtual void RunEventInterceptors(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryEventInterceptors.ContainsKey(interceptorType))
            {
                RunEvents(dictionaryEventInterceptors[interceptorType], input);
                if (canClearList)
                {
                    dictionaryEventInterceptors.Remove(interceptorType);
                }
            }
        }
        protected virtual void RunCustomEventInterceptors(IPipelineMessage input, bool postOperation = true)
        {
            if (postOperation)
            {
                if (postEventCustomInterceptors.AnySafe())
                {
                    foreach (var ci in postEventCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        RunEvents(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                    }
                }
            }
            else
            {
                if (preEventCustomInterceptors.AnySafe())
                {
                    foreach (var ci in preEventCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        RunEvents(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                    }
                }
            }
        }
        protected virtual void RunEvents(IList<EventHandler<IPipelineMessage, EventArgs>> _handlers, IPipelineMessage input)
        {
            if (_handlers.AnySafe())
            {
                foreach (var handler in _handlers)
                {
                    if (handler == null)
                    {
                        continue;
                    }
                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-event-start", $"operation:{handler.GetType().Name}");
                    try
                    {
                        Task.Factory.StartNew(() => handler(input, EventArgs.Empty));
                    }
                    finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-event-end", $"operation:{handler.GetType().Name}"); }
                }
            }
        }
        private void RunRollbackOperations(IPipelineMessage input)
        {
            if (executedOperations.AnySafe())
            {
                foreach (var executedOperation in executedOperations.Reverse<IOperation>())
                {
                    if (executedOperation == null)
                    {
                        continue;
                    }

                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-rollback-operation-start", "operation:" + executedOperation.GetType().Name);
                    try
                    {
                        executedOperation.Rollback(input);
                    }
                    catch (Exception ex) { TelemetryHelper.Execute(TelemetryLevels.Error, "pipe-pipeline-rollback-failure", ex); }
                    finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-rollback-operation-end", "operation:" + executedOperation.GetType().Name); }
                }
            }
        }

        #endregion
    }
}
