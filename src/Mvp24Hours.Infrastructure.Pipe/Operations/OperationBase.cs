//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Pipe;
using Mvp24Hours.Helpers;
using System;

namespace Mvp24Hours.Infrastructure.Pipe.Operations
{
    /// <summary>  
    /// Abstraction of base operations
    /// </summary>
    public abstract class OperationBase : IOperation
    {
        #region [ Ctors ]
        protected OperationBase()
            : this(ServiceProviderHelper.GetService<INotificationContext>())
        {
        }

        [ActivatorUtilitiesConstructor]
        protected OperationBase(INotificationContext _notificationContext)
        {
            notificationContext = _notificationContext ?? throw new ArgumentNullException(nameof(_notificationContext), "Notification context is mandatory.");
        }
        #endregion

        #region [ Properties / Fields ]
        private readonly INotificationContext notificationContext;
        /// <summary>
        /// Notification context based on individual HTTP request
        /// </summary>
        protected INotificationContext NotificationContext => notificationContext;
        public virtual bool IsRequired => false;
        #endregion

        #region [ Methods ]
        public abstract void Execute(IPipelineMessage input);
        #endregion
    }
}
