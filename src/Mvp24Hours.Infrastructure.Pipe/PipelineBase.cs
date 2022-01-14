//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Helpers;
using System;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine base
    /// </summary>
    public abstract class PipelineBase
    {
        #region [ Ctor ]
        protected PipelineBase()
            : this(true)
        {
        }
        protected PipelineBase(string token)
            : this(token, true)
        {
        }
        protected PipelineBase(bool isBreakOnFail)
            : this(null, isBreakOnFail)
        {
        }
        protected PipelineBase(string token, bool isBreakOnFail)
        {
            this.IsBreakOnFail = isBreakOnFail;
            this.Token = token;

            Context = ServiceProviderHelper.GetService<INotificationContext>();

            if (Context == null)
            {
                throw new ArgumentNullException(string.Empty, "Notification context is mandatory.");
            }
        }
        #endregion

        #region [ Fields / Properties ]

        #region [ Fields ]
        protected readonly bool IsBreakOnFail;
        protected string Token { get; set; }
        protected IPipelineMessage Message { get; set; }
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

        public IPipelineMessage GetMessage() => Message;
        public string GetToken() => Token;

        #endregion
    }
}
