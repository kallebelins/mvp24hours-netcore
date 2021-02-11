//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Core.Enums;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Helpers;
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
            : this(Guid.NewGuid().ToString(), isBreakOnFail)
        {
        }
        public PipelineAsync(string token, bool isBreakOnFail)
        {
            this._isBreakOnFail = isBreakOnFail;
            this._token = token ?? Guid.NewGuid().ToString();

            Context = HttpContextHelper.GetService<INotificationContext>();

            if (Context == null)
                throw new ArgumentNullException("Notification context is mandatory.");
        }
        #endregion

        #region [ Fields / Properties ]

        #region [ Fields ]
        private List<IOperationAsync> _operations = new List<IOperationAsync>();
        private bool _isBreakOnFail;
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
        public IPipelineAsync AddAsync<T>() where T : IOperationAsync, new()
        {
            return AddAsync(new T());
        }
        public IPipelineAsync AddAsync(IOperationAsync operation)
        {
            this._operations.Add(operation);
            return this;
        }
        public async Task<IPipelineMessage> Execute(IPipelineMessage input)
        {
            return await this._operations.Aggregate(Task.FromResult(input), async (current, operation) =>
            {
                var result = await current;
                result.SetToken(this._token);
                if (!operation.IsRequired && (!result.IsSuccess || !IsValidContext) && this._isBreakOnFail)
                    return result;
                if (result.IsLocked)
                    return result;
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
