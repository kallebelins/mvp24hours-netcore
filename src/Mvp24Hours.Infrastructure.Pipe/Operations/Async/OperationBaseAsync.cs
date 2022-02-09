//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Helpers;
using System;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Pipe.Operations
{
    /// <summary>  
    /// Abstraction of asynchronous base operations
    /// </summary>
    public abstract class OperationBaseAsync : IOperationAsync
    {
        #region [ Ctors ]
        public OperationBaseAsync()
        {
            NotificationContext = ServiceProviderHelper.GetService<INotificationContext>();

            if (NotificationContext == null)
            {
                throw new ArgumentNullException("Notification context is mandatory.");
            }
        }
        #endregion

        #region [ Props ]
        /// <summary>
        /// Notification context based on individual HTTP request
        /// </summary>
        protected INotificationContext NotificationContext { get; private set; }
        public virtual bool IsRequired => false;
        #endregion

        #region [ Methods ]
        public abstract Task<IPipelineMessage> ExecuteAsync(IPipelineMessage input);
        #endregion
    }
}
