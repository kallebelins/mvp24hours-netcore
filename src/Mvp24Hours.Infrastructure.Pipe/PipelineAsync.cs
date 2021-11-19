//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
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
    public class PipelineAsync : IPipelineAsync
    {
        #region [ Ctor ]
        public PipelineAsync()
            : this(true)
        {
        }
        public PipelineAsync(string token)
            : this(token, true)
        {
        }
        public PipelineAsync(bool isBreakOnFail)
            : this(null, isBreakOnFail)
        {
        }
        public PipelineAsync(string token, bool isBreakOnFail)
        {
            this._isBreakOnFail = isBreakOnFail;
            this._token = token;

            Context = ServiceProviderHelper.GetService<INotificationContext>();

            if (Context == null)
            {
                throw new ArgumentNullException("Notification context is mandatory.");
            }
        }
        #endregion

        #region [ Fields / Properties ]

        #region [ Fields ]
        private readonly IList<IOperationAsync> operations = new List<IOperationAsync>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> preCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>>();
        private readonly IList<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>> postCustomInterceptors = new List<KeyValuePair<Func<IPipelineMessage, bool>, IOperationAsync>>();
        private readonly IDictionary<PipelineInterceptorType, IList<IOperationAsync>> dictionaryInterceptors = new Dictionary<PipelineInterceptorType, IList<IOperationAsync>>();
        private readonly bool _isBreakOnFail;
        private IPipelineMessage _pipelineMessage = null;
        private string _token;
        #endregion

        /// <summary>
        /// Notification context based on individual HTTP request
        /// </summary>
        protected INotificationContext Context { get; private set; }
        /// <summary>
        /// Indicates whether there are failures in the notification context
        /// </summary>
        protected bool IsValidContext => !Context.HasErrorNotifications;
        #endregion

        #region [ Methods ]
        public IPipelineMessage GetMessage() => _pipelineMessage;
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
                    throw new ArgumentNullException("Operation not found. Check if it has been registered in this context.");
                }
            }
            return Add(instance);
        }
        public IPipelineAsync Add(IOperationAsync operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("Operation has not been defined or is null.");
            }
            this.operations.Add(operation);
            return this;
        }
        public IPipelineAsync Add(Action<IPipelineMessage> action, bool isRequired = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action is mandatory.");
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
                    throw new ArgumentNullException("Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, pipelineInterceptor);
        }
        public IPipelineAsync AddInterceptors(IOperationAsync operation, PipelineInterceptorType pipelineInterceptor = PipelineInterceptorType.PostOperation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("Operation has not been defined or is null.");
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
                throw new ArgumentNullException("Action is mandatory.");
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
                    throw new ArgumentNullException("Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, condition, postOperation);
        }
        public IPipelineAsync AddInterceptors(IOperationAsync operation, Func<IPipelineMessage, bool> condition, bool postOperation = true)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("Operation has not been defined or is null.");
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
                throw new ArgumentNullException("Action is mandatory.");
            }
            if (condition == null)
            {
                throw new ArgumentNullException("Condition is mandatory.");
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
        public async Task<IPipelineAsync> ExecuteAsync(IPipelineMessage input = null)
        {
            _pipelineMessage = input ?? _pipelineMessage;
            _pipelineMessage = await RunOperationsAsync(this.operations, _pipelineMessage);
            return this;
        }
        internal async Task<IPipelineMessage> RunOperationsAsync(IList<IOperationAsync> _operations, IPipelineMessage input, bool onlyOperationDefault = false)
        {
            if (!_operations.AnyOrNotNull())
            {
                return input;
            }

            if (input == null)
            {
                input = new PipelineMessage();
            }

            if (!_token.HasValue())
            {
                _token = input.Token.HasValue() ? input.Token : Guid.NewGuid().ToString();
            }

            if (!onlyOperationDefault && dictionaryInterceptors.ContainsKey(PipelineInterceptorType.FirstOperation))
            {
                await RunOperationsAsync(dictionaryInterceptors[PipelineInterceptorType.FirstOperation], input, true);
            }

            _ = await _operations.Aggregate(Task.FromResult(input), async (currentAsync, operation) =>
            {
                var current = await currentAsync;

                current.SetToken(this._token);

                if (!onlyOperationDefault)
                {
                    if (current.IsFaulty || !IsValidContext)
                    {
                        if (dictionaryInterceptors.ContainsKey(PipelineInterceptorType.Faulty))
                        {
                            await RunOperationsAsync(dictionaryInterceptors[PipelineInterceptorType.Faulty], current, true);
                            dictionaryInterceptors.Remove(PipelineInterceptorType.Faulty);
                        }

                        if (this._isBreakOnFail)
                        {
                            return current;
                        }
                    }

                    if (current.IsLocked)
                    {
                        if (dictionaryInterceptors.ContainsKey(PipelineInterceptorType.Locked))
                        {
                            await RunOperationsAsync(dictionaryInterceptors[PipelineInterceptorType.Locked], current, true);
                            dictionaryInterceptors.Remove(PipelineInterceptorType.Locked);
                        }

                        if (!operation.IsRequired)
                        {
                            return current;
                        }
                    }
                }

                try
                {
                    // pre-operation
                    if (!onlyOperationDefault)
                    {
                        if (preCustomInterceptors.AnyOrNotNull())
                        {
                            foreach (var ci in preCustomInterceptors)
                            {
                                if (ci.Key.Invoke(input))
                                {
                                    await RunOperationsAsync(new List<IOperationAsync> { ci.Value }, current, true);
                                }
                            }
                        }
                        if (dictionaryInterceptors.ContainsKey(PipelineInterceptorType.PreOperation))
                        {
                            await RunOperationsAsync(dictionaryInterceptors[PipelineInterceptorType.PreOperation], current, true);
                        }
                    }

                    // operation
                    await operation.ExecuteAsync(current);

                    // post-operation
                    if (!onlyOperationDefault)
                    {
                        if (dictionaryInterceptors.ContainsKey(PipelineInterceptorType.PostOperation))
                        {
                            await RunOperationsAsync(dictionaryInterceptors[PipelineInterceptorType.PostOperation], current, true);
                        }

                        if (postCustomInterceptors.AnyOrNotNull())
                        {
                            foreach (var ci in postCustomInterceptors)
                            {
                                if (ci.Key.Invoke(input))
                                {
                                    await RunOperationsAsync(new List<IOperationAsync> { ci.Value }, current, true);
                                }
                            }
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

            if (!onlyOperationDefault && dictionaryInterceptors.ContainsKey(PipelineInterceptorType.LastOperation))
            {
                await RunOperationsAsync(dictionaryInterceptors[PipelineInterceptorType.LastOperation], input, true);
            }

            return input;
        }

        #endregion
    }
}
