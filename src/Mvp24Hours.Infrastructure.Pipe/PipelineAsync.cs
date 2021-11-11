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
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Pipe.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// <see cref="Mvp24Hours.Core.Contract.Infrastructure.Pipe.IPipelineAsync"/>
    /// </summary>
    [DebuggerStepThrough]
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
        private readonly IList<IOperationAsync> _operations = new List<IOperationAsync>();
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
        public IPipelineAsync AddAsync<T>() where T : IOperationAsync
        {
            IOperationAsync instance = ServiceProviderHelper.GetService<T>();
            if (instance == null)
            {
                Type type = typeof(T);
                if (type.IsClass && !type.IsAbstract)
                {
                    return AddAsync(Activator.CreateInstance<T>());
                }
                else
                {
                    throw new ArgumentNullException("Operation not found. Check if it has been registered in this context.");
                }
            }
            return AddAsync(instance);
        }
        public IPipelineAsync AddAsync(IOperationAsync operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("Operation has not been defined or is null.");
            }
            this._operations.Add(operation);
            return this;
        }
        public IPipelineAsync AddAsync(Action<IPipelineMessage> action, bool isRequired = false)
        {
            if (action == null)
            {
                throw new ArgumentNullException("Action is mandatory.");
            }
            this._operations.Add(new OperationActionAsync(action, isRequired));
            return this;
        }
        public async Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input = null)
        {
            if (input == null)
            {
                input = new PipelineMessage();
            }

            if (!_token.HasValue())
            {
                _token = input.Token.HasValue() ? input.Token : Guid.NewGuid().ToString();
            }

            return await this._operations.Aggregate(Task.FromResult(input), async (current, operation) =>
            {
                var result = await current;
                result.SetToken(this._token);
                if (!operation.IsRequired && (!result.IsSuccess || !IsValidContext) && this._isBreakOnFail)
                {
                    return result;
                }

                if (!operation.IsRequired && result.IsLocked)
                {
                    return result;
                }

                try
                {
                    return await operation.Execute(result);
                }
                catch (Exception ex)
                {
                    result.Messages.Add(new MessageResult((ex?.InnerException ?? ex).Message, MessageType.Error));
                    input.AddContent(ex);
                }
                return result;
            });
        }
        #endregion
    }
}
