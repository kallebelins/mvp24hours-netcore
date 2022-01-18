//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Core.Contract.Infrastructure.Logging;
using Mvp24Hours.Helpers;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Base service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public abstract class RepositoryServiceBase<TUoW>
        where TUoW : IUnitOfWork
    {
        #region [ Properties ]

        private IUnitOfWork unitOfWork = null;
        private ILoggingService logger = null;
        private INotificationContext context = null;

        /// <summary>
        /// Gets unit of work instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual TUoW UnitOfWork => (TUoW)(unitOfWork ??= ServiceProviderHelper.GetService<TUoW>());

        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging => logger ??= ServiceProviderHelper.GetService<ILoggingService>();

        /// <summary>
        /// Gets instance of notification context
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual INotificationContext NotificationContext => context ??= ServiceProviderHelper.GetService<INotificationContext>();

        /// <summary>
        /// Maximum amount returned in query
        /// </summary>
        protected virtual int MaxQtyByQueryPage => ConfigurationPropertiesHelper.MaxQtyByQueryPage;

        #endregion
    }
}
