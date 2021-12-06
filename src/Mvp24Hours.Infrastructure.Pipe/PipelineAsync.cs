//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.Enums.Infrastructure;
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
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
        public PipelineAsync()
            : base()
        {
        }
        public PipelineAsync(string token)
            : base(token)
        {
        }
        public PipelineAsync(bool isBreakOnFail)
            : base(isBreakOnFail)
        {
        }
        public PipelineAsync(string token, bool isBreakOnFail)
            : base(token, isBreakOnFail)
        {
        }
        #endregion

        #region [ Fields / Properties ]
        private readonly IList<IOperationAsync> operations = new List<IOperationAsync>();

        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> preCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> postCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>>();
        private readonly IDictionary<PipelineInterceptorType, IList<IOperationAsync>> dictionaryInterceptors = new Dictionary<PipelineInterceptorType, IList<IOperationAsync>>();

        private readonly IDictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>> dictionaryEventInterceptors = new Dictionary<PipelineInterceptorType, IList<EventHandler<IPipelineMessage, EventArgs>>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> preEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>> postEventCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, EventHandler<IPipelineMessage, EventArgs>>>();
        #endregion

        #region [ Methods ]
        public IPipelineAsync Add<T>() where T : IOperationAsync
        {
            IOperationAsync instance = ServiceProviderHelper.GetService<T>();
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

        public IPipelineAsync AddInterceptors<T>(PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation) where T : IOperationAsync
        {
            IOperationAsync instance = ServiceProviderHelper.GetService<T>();
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
            if (!this.dictionaryInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryInterceptors.Add(pipelineInterceptor, new List<IOperationAsync>());
            }
            this.dictionaryInterceptors[pipelineInterceptor].Add(operation);
            return this;
        }
        public IPipelineAsync AddInterceptors(Action<IPipelineMessage> action, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "Action is mandatory.");
            }
            if (!this.dictionaryInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryInterceptors.Add(pipelineInterceptor, new List<IOperationAsync>());
            }
            this.dictionaryInterceptors[pipelineInterceptor].Add(new OperationActionAsync(action));
            return this;
        }
        public IPipelineAsync AddInterceptors<T>(Func<IPipelineMessage, bool> condition, bool postOperation = true) where T : IOperationAsync
        {
            IOperationAsync instance = ServiceProviderHelper.GetService<T>();
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

            if (!this.dictionaryEventInterceptors.ContainsKey(pipelineInterceptor))
            {
                this.dictionaryEventInterceptors.Add(pipelineInterceptor, new List<EventHandler<IPipelineMessage, EventArgs>>());
            }
            this.dictionaryEventInterceptors[pipelineInterceptor].Add(handler);
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

        public async Task<IPipelineAsync> ExecuteAsync(IPipelineMessage input = null)
        {
            Message = input ?? Message;
            Message = await RunOperationsAsync(this.operations, Message);
            return this;
        }
        internal virtual async Task<IPipelineMessage> RunOperationsAsync(IList<IOperationAsync> _operations, IPipelineMessage input, bool onlyOperationDefault = false)
        {
            if (!_operations.AnyOrNotNull())
            {
                return input;
            }

            if (input == null)
            {
                input = new PipelineMessage();
            }

            if (!Token.HasValue())
            {
                Token = input.Token.HasValue() ? input.Token : Guid.NewGuid().ToString();
            }

            if (!onlyOperationDefault)
            {
                await RunEventInterceptorsAsync(input, PipelineInterceptorType.FirstOperation);
                await RunOperationInterceptorsAsync(input, PipelineInterceptorType.FirstOperation);
            }

            _ = await _operations.Aggregate(Task.FromResult(input), async (currentAsync, operation) =>
            {
                var current = await currentAsync;

                current.SetToken(this.Token);

                if (!onlyOperationDefault)
                {
                    if ((current.IsFaulty || !IsValidContext) && this.IsBreakOnFail)
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
                    await operation.ExecuteAsync(current);

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

                        if (current.IsFaulty || !IsValidContext)
                        {
                            await RunEventInterceptorsAsync(input, PipelineInterceptorType.Faulty, true);
                            await RunOperationInterceptorsAsync(current, PipelineInterceptorType.Faulty, true);
                        }
                    }

                    return current;
                }
                catch (Exception ex)
                {
                    current.Messages.Add(new MessageResult((ex?.InnerException ?? ex).Message, MessageType.Error));
                    input.AddContent(ex);
                }
                return current;
            });

            if (!onlyOperationDefault && (!input.IsFaulty || IsValidContext))
            {
                await RunEventInterceptorsAsync(input, PipelineInterceptorType.LastOperation);
                await RunOperationInterceptorsAsync(input, PipelineInterceptorType.LastOperation);
            }

            return input;
        }

        protected virtual async Task RunOperationInterceptorsAsync(IPipelineMessage input, PipelineInterceptorType interceptorType, bool canClearList = false)
        {
            if (dictionaryInterceptors.ContainsKey(interceptorType))
            {
                await RunOperationsAsync(dictionaryInterceptors[interceptorType], input, true);
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
                if (postCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in postCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            await RunOperationsAsync(new List<IOperationAsync> { ci.Value }, input, true);
                        }
                    }
                }
            }
            else
            {
                if (preCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in preCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            await RunOperationsAsync(new List<IOperationAsync> { ci.Value }, input, true);
                        }
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
                if (postEventCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in postEventCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            await RunEventsAsync(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                        }
                    }
                }
            }
            else
            {
                if (preEventCustomInterceptors.AnyOrNotNull())
                {
                    foreach (var ci in preEventCustomInterceptors)
                    {
                        if (ci.Key.Invoke(input))
                        {
                            await RunEventsAsync(new List<EventHandler<IPipelineMessage, EventArgs>> { ci.Value }, input);
                        }
                    }
                }
            }
        }
        protected virtual async Task RunEventsAsync(IList<EventHandler<IPipelineMessage, EventArgs>> _handlers, IPipelineMessage input)
        {
            if (_handlers.AnyOrNotNull())
            {
                foreach (var handler in _handlers)
                {
                    if (handler == null)
                    {
                        continue;
                    }

                    await Task.Factory.StartNew(() => handler(input, EventArgs.Empty));
                }
            }
        }

        #endregion
    }
}
