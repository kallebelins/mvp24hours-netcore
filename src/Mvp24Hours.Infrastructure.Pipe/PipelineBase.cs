//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using System;

namespace Mvp24Hours.Infrastructure.Pipe
{
    /// <summary>
    /// Defines pipeline engine base
    /// </summary>
    public abstract class PipelineBase
    {
        #region [ Ctor ]
        protected PipelineBase(INotificationContext notificationContext, bool isBreakOnFail = false)
        {
            this.IsBreakOnFail = isBreakOnFail;

            Context = notificationContext;

            if (Context == null)
            {
                throw new ArgumentNullException(string.Empty, "Notification context is mandatory.");
            }
        }
        #endregion

        #region [ Fields / Properties ]

        #region [ Fields ]
        protected readonly bool IsBreakOnFail;
        protected IPipelineMessage Message { get; set; }
        #endregion

        /// <summary>
        /// Notification context based on individual HTTP request
        /// </summary>
        protected INotificationContext Context { get; private set; }
        /// <summary>
        /// Indicates whether there are failures in the notification context
        /// </summary>
        protected bool IsValidContext => !Context?.HasErrorNotifications ?? true;
        #endregion

        #region [ Methods ]

        public IPipelineMessage GetMessage() => Message;

        #endregion
    }
}
