//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Log;
using System.Threading.Tasks;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base business class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryServiceAsyncBase<T, U>
        where T : class, IEntityBase
        where U : IUnitOfWorkAsync
    {
        #region [ Properties ]

        private IUnitOfWorkAsync unitOfWork = null;

        /// <summary>
        /// Gets repository instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual U UnitOfWork
        {
            get { return (U)(unitOfWork ?? (unitOfWork = HttpContextHelper.GetService<U>())); }
        }

        ILoggingService logger = null;

        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging
        {
            get { return logger ?? (logger = LoggingService.GetLoggingService()); }
        }

        /// <summary>
        /// Maximum amount returned in query
        /// </summary>
        protected virtual int MaxQtyByQueryPage
        {
            get
            {
                return ConfigurationPropertiesHelper.MaxQtyByQueryPage;
            }
        }

        #endregion

        #region [ Methods ]

        protected virtual Task<R> TaskResult<R>(R obj)
        {
            return Task.FromResult<R>(obj);
        }

        #endregion
    }
}
