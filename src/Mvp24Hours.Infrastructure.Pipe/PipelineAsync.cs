//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mvp24Hours.Core.Contract.Application.Pipe.Async;
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
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Pipe.IPipelineAsync"/>
    /// </summary>
    public class PipelineAsync : PipelineBase, IPipelineAsync
    {
        #region [ Ctor ]

        public PipelineAsync(IServiceProvider _provider = null)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipelineasync-ctor");
            this.provider = _provider;

            var options = _provider?.GetService<IOptions<PipelineOptions>>()?.Value;
            if (options != null)
            {
                this.IsBreakOnFail = options.IsBreakOnFail;
            }

            operations = new List<IOperationAsync>();

            preCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>>();
            postCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>>();
            dictionaryInterceptors = new Dictionary<PipelineInterceptorType, IList<IOperationAsync>>();

            dictionaryEventInterceptors = new Dictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>>();
            preEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
            postEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
        }

        #endregion

        #region [ Fields / Properties ]
        private readonly IServiceProvider provider;
        private readonly IList<IOperationAsync> operations;

        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> preCustomInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> postCustomInterceptors;
        private readonly IDictionary<PipelineInterceptorType, IList<IOperationAsync>> dictionaryInterceptors;

        private readonly IDictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>> dictionaryEventInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> preEventCustomInterceptors;
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> postEventCustomInterceptors;
        #endregion

        #region [ Methods ]

        #region [ Get ]
        public IList<IOperationAsync> GetOperations() => operations;
        public IDictionary<PipelineInterceptorType, IList<IOperationAsync>> GetInterceptors() => dictionaryInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> GetPreInterceptors() => preCustomInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> GetPostInterceptors() => postCustomInterceptors;
        public IDictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>> GetEvents() => dictionaryEventInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> GetPreEvents() => preEventCustomInterceptors;
        public IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> GetPostEvents() => postEventCustomInterceptors;
        #endregion

        public IPipelineAsync Add<T>() where T : class, IOperationAsync
        {
            IOperationAsync instance = provider?.GetService<T>();
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
        public IPipelineAsync Add(IOperationAsync operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            this.operations.Add(operation);
            return this;
        }
        public IPipelineAsync Add(Action<IPipelineMessage> action, bool isRequired = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            this.operations.Add(new OperationActionAsync(action, isRequired));
            return this;
        }

        public IPipelineAsync AddBuilder<T>() where T : class, IPipelineBuilderAsync
        {
            IPipelineBuilderAsync pipelineBuilder = provider?.GetService<T>();
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
            return pipelineBuilder.Builder(this);
        }
        public IPipelineAsync AddBuilder(IPipelineBuilderAsync pipelineBuilder)
        {
            if (pipelineBuilder == null)
            {
                throw new ArgumentNullException(nameof(pipelineBuilder), "PipelineBuilder has not been defined or is null.");
            }
            pipelineBuilder.Builder(this);
            return this;
        }

        public IPipelineAsync AddInterceptors<T>(PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation) where T : class, IOperationAsync
        {
            IOperationAsync instance = provider?.GetService<T>();
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
        public IPipelineAsync AddInterceptors(IOperationAsync operation, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            if (!dictionaryInterceptors.TryGetValue(pipelineInterceptor, out IList<IOperationAsync> value))
            {
                value = new List<IOperationAsync>();
                this.dictionaryInterceptors.Add(pipelineInterceptor, value);
            }

            value.Add(operation);
            return this;
        }
        public IPipelineAsync AddInterceptors(Action<IPipelineMessage> action, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            if (!dictionaryInterceptors.TryGetValue(pipelineInterceptor, out IList<IOperationAsync> value))
            {
                value = new List<IOperationAsync>();
                this.dictionaryInterceptors.Add(pipelineInterceptor, value);
            }

            value.Add(new OperationActionAsync(action));
            return this;
        }
        public IPipelineAsync AddInterceptors<T>(Func<IPipelineMessage, bool> condition, bool postOperation = true) where T : class, IOperationAsync
        {
            IOperationAsync instance = provider?.GetService<T>();
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
        public IPipelineAsync AddInterceptors(IOperationAsync operation, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation), "Operation has not been defined or is null.");
            }
            if (postOperation)
            {
                this.postCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>(condition, operation));
            }
            else
            {
                this.preCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>(condition, operation));
            }
            return this;
        }
        public IPipelineAsync AddInterceptors(Action<IPipelineMessage> action, Func<IPipelineMessage, bool> condition, bool postOperation = true)
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
                this.postCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>(condition, new OperationActionAsync(action)));
            }
            else
            {
                this.preCustomInterceptors.Add(new KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>(condition, new OperationActionAsync(action)));
            }
            return this;
        }
        public IPipelineAsync AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Handler has not been defined or is null.");
            }

            if (!dictionaryEventInterceptors.TryGetValue(pipelineInterceptor, out IList<EventHandler<IPipelineMessage, EventArgs>> value))
            {
                value = new List<EventHandler<IPipelineMessage, EventArgs>>();
                this.dictionaryEventInterceptors.Add(pipelineInterceptor, value);
            }

            value.Add(handler);
            return this;
        }
        public IPipelineAsync AddInterceptors(EventHandler<IPipelineMessage, EventArgs> handler, Func<IPipelineMessage, bool> condition, bool postOperation = true)
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

        public async Task ExecuteAsync(IPipelineMessage input = null)
        {
            TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipelineasync-executeasync-start");
            try
            {
                Message = input ?? Message;
                Message = await RunOperationsAsync(this.operations, Message);
            }
            finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipelineasync-executeasync-end"); }
        }
        internal virtual async Task<IPipelineMessage> RunOperationsAsync(IList<IOperationAsync> _operations, IPipelineMessage input, bool onlyOperationDefault = false)
        {
            if (!_operations.AnySafe())
            {
                return input;
            }

            input ??= new PipelineMessage();

            if (!onlyOperationDefault)
            {
                await RunEventInterceptorsAsync(input, PipelineInterceptorType.FirstOperation);
                await RunOperationInterceptorsAsync(input, PipelineInterceptorType.FirstOperation);
            }

            _ = await _operations.Aggregate(Task.FromResult(input), async (currentAsync, operation) =>
            {
                var current = await currentAsync;

                if (!onlyOperationDefault)
                {
                    if ((current.IsFaulty) && this.IsBreakOnFail)
                    {
                        return current;
                    }

                    if (current.IsLocked && !operation.IsRequired)
                    {
                        return current;
                    }
                }

                try
                {
                    // pre-operation
                    if (!onlyOperationDefault)
                    {
                        // events
                        await RunCustomEventInterceptorsAsync(input, false);
                        await RunEventInterceptorsAsync(input, PipelineInterceptorType.PreOperation);

                        // operations
                        await RunCustomOperationInterceptorsAsync(input, false);
                        await RunOperationInterceptorsAsync(input, PipelineInterceptorType.PreOperation);
                    }

                    // operation                    
                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-operation-start", $"operation:{operation.GetType().Name}");
                    try
                    {
                        await operation.ExecuteAsync(current);
                    }
                    finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipeline-execute-operation-end", $"operation:{operation.GetType().Name}"); }

                    // post-operation
                    if (!onlyOperationDefault)
                    {
                        // events
                        await RunEventInterceptorsAsync(input, PipelineInterceptorType.PostOperation);
                        await RunCustomEventInterceptorsAsync(input);

                        // operations
                        await RunOperationInterceptorsAsync(current, PipelineInterceptorType.PostOperation);
                        await RunCustomOperationInterceptorsAsync(current);

                        if (current.IsLocked)
                        {
                            await RunEventInterceptorsAsync(input, PipelineInterceptorType.Locked, true);
                            await RunOperationInterceptorsAsync(current, PipelineInterceptorType.Locked, true);
                        }

                        if (current.IsFaulty)
                        {
                            await RunEventInterceptorsAsync(input, PipelineInterceptorType.Faulty, true);
                            await RunOperationInterceptorsAsync(current, PipelineInterceptorType.Faulty, true);
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
                await RunEventInterceptorsAsync(input, PipelineInterceptorType.LastOperation);
                await RunOperationInterceptorsAsync(input, PipelineInterceptorType.LastOperation);
            }

            return input;
        }

        protected virtual async Task RunOperationInterceptorsAsync(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryInterceptors.TryGetValue(interceptorType, out IList<IOperationAsync> value))
            {
                await RunOperationsAsync(value, input, true);
                if (canClearList)
                {
                    dictionaryInterceptors.Remove(interceptorType);
                }
            }
        }
        protected virtual async Task RunCustomOperationInterceptorsAsync(IPipelineMessage input, bool postOperation = true)
        {
            if (postOperation)
            {
                if (postCustomInterceptors.AnySafe())
                {
                    foreach (var ci in postCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        await RunOperationsAsync(new List<IOperationAsync> { ci.Value }, input, true);
                    }
                }
            }
            else
            {
                if (preCustomInterceptors.AnySafe())
                {
                    foreach (var ci in preCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        await RunOperationsAsync(new List<IOperationAsync> { ci.Value }, input, true);
                    }
                }
            }
        }
        protected virtual async Task RunEventInterceptorsAsync(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryEventInterceptors.ContainsKey(interceptorType))
            {
                await RunEventsAsync(dictionaryEventInterceptors[interceptorType], input);
                if (canClearList)
                {
                    dictionaryEventInterceptors.Remove(interceptorType);
                }
            }
        }
        protected virtual async Task RunCustomEventInterceptorsAsync(IPipelineMessage input, bool postOperation = true)
        {
            if (postOperation)
            {
                if (postEventCustomInterceptors.AnySafe())
                {
                    foreach (var ci in postEventCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        await RunEventsAsync(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                    }
                }
            }
            else
            {
                if (preEventCustomInterceptors.AnySafe())
                {
                    foreach (var ci in preEventCustomInterceptors.Where(ci => ci.Key.Invoke(input)))
                    {
                        await RunEventsAsync(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                    }
                }
            }
        }
        protected virtual async Task RunEventsAsync(IList<EventHandler<IPipelineMessage, EventArgs>> _handlers, IPipelineMessage input)
        {
            if (_handlers.AnySafe())
            {
                foreach (var handler in _handlers)
                {
                    if (handler == null)
                    {
                        continue;
                    }

                    TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipelineasync-executeasync-eventasync-start", $"operation:{handler.GetType().Name}");
                    try
                    {
                        await Task.Factory.StartNew(() => handler(input, EventArgs.Empty));
                    }
                    finally { TelemetryHelper.Execute(TelemetryLevels.Verbose, "pipe-pipelineasync-executeasync-eventasync-end", $"operation:{handler.GetType().Name}"); }
                }
            }
        }

        #endregion
    }
}
