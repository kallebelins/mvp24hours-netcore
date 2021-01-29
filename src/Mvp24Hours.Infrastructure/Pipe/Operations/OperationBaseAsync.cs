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
    /// Abstraction of asynchronous mapping operations
    /// </summary>
    public abstract class OperationBaseAsync : IOperationAsync
    {
        #region [ Ctors ]
        public OperationBaseAsync()
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
        public abstract Task<IPipelineMessage> Execute(IPipelineMessage input);
        #endregion
    }
}
