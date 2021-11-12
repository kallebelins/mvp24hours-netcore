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
using Mvp24Hours.Core.Extensions;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Pipe.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Pipe.IPipeline"/>
    /// </summary>
    [DebuggerStepThrough]
    public class Pipeline : IPipeline
    {
        #region [ Ctor ]
        public Pipeline()
            : this(true)
        {
        }
        public Pipeline(string token)
            : this(token, true)
        {
        }
        public Pipeline(bool isBreakOnFail)
            : this(null, isBreakOnFail)
        {
        }
        public Pipeline(string token, bool isBreakOnFail)
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
        private readonly IList<IOperation> operations = new List<IOperation>();
        private readonly IList<IOperation> preOperationsInterceptors = new List<IOperation>();
        private readonly IList<IOperation> postOperationsInterceptors = new List<IOperation>();
        private readonly bool _isBreakOnFail;
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
        public IPipeline Add<T>() where T : IOperation
        {
            IOperation instance = ServiceProviderHelper.GetService<T>();
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
        public IPipeline Add(IOperation operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("Operation has not been defined or is null.");
            }
            this.operations.Add(operation);
            return this;
        }
        public IPipeline Add(Action<IPipelineMessage> action, bool isRequired = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action is mandatory.");
            }
            this.operations.Add(new OperationAction(action, isRequired));
            return this;
        }
        public IPipeline AddInterceptors<T>(bool postOperation = false) where T : IOperation
        {
            IOperation instance = ServiceProviderHelper.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return AddInterceptors(Activator.CreateInstance<T>(), postOperation);
                }
                else
                {
                    throw new ArgumentNullException("Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddInterceptors(instance, postOperation);
        }
        public IPipeline AddInterceptors(IOperation operation, bool postOperation = false)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("Operation has not been defined or is null.");
            }
            if (postOperation)
            {
                this.postOperationsInterceptors.Add(operation);
            }
            else
            {
                this.preOperationsInterceptors.Add(operation);
            }
            return this;
        }
        public IPipeline AddInterceptors(Action<IPipelineMessage> action, bool postOperation = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action is mandatory.");
            }
            if (postOperation)
            {
                this.postOperationsInterceptors.Add(new OperationAction(action));
            }
            else
            {
                this.preOperationsInterceptors.Add(new OperationAction(action));
            }
            return this;
        }
        public IPipelineMessage Execute(IPipelineMessage input = null)
        {
            return RunOperations(this.operations, input);
        }
        internal IPipelineMessage RunOperations(IList<IOperation> _operations, IPipelineMessage input = null, bool onlyOperationDefault = false)
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

            return _operations.Aggregate(input, (current, operation) =>
            {
                current.SetToken(this._token);
                if (!operation.IsRequired && (!current.IsSuccess || !IsValidContext) && this._isBreakOnFail)
                {
                    return current;
                }

                if (!operation.IsRequired && current.IsLocked)
                {
                    return current;
                }

                try
                {
                    // pre-operation
                    if (!onlyOperationDefault)
                    {
                        RunOperations(this.preOperationsInterceptors, current, true);
                    }

                    // operation
                    operation.Execute(current);

                    // post-operation
                    if (!onlyOperationDefault)
                    {
                        RunOperations(this.postOperationsInterceptors, current, true);
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
        }

        #endregion
    }
}
