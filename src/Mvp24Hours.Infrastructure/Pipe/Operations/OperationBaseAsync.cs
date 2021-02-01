//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Infrastructure.Helpers;
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
            NotificationContext = HttpContextHelper.GetService<INotificationContext>();

            if (NotificationContext == null)
                throw new ArgumentNullException("Notification context is mandatory.");
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
        public abstract Task<IPipelineMessage> Execute(IPipelineMessage input);
        #endregion
    }
}
