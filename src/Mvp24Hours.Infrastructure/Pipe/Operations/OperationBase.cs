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

namespace Mvp24Hours.Infrastructure.Pipe.Operations
{
    /// <summary>  
    /// Abstraction of mapping operations
    /// </summary>
    public abstract class OperationBase : IOperation
    {
        #region [ Ctors ]
        public OperationBase()
        {
            Context = HttpContextHelper.GetService<INotificationContext>();

            if (Context == null)
                throw new ArgumentNullException("Notification context is mandatory.");
        }
        #endregion

        #region [ Props ]
        protected INotificationContext Context { get; private set; }
        public bool IsValid => !Context.HasNotifications;
        public virtual bool IsRequired => false;
        #endregion

        #region [ Methods ]
        public abstract IPipelineMessage Execute(IPipelineMessage input);
        #endregion
    }
}
