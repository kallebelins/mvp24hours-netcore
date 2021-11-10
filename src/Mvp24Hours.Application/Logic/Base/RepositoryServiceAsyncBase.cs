//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Domain.Validations;
using Mvp24Hours.Infrastructure.Helpers;
using Mvp24Hours.Infrastructure.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Asynchronous base service for using repository and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public abstract class RepositoryServiceAsyncBase<TEntity, TUoW>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Properties ]

        private IUnitOfWorkAsync unitOfWork = null;

        /// <summary>
        /// Gets repository instance
        /// </summary>
        /// <returns>T</returns>
        protected virtual TUoW UnitOfWork
        {
            get { return (TUoW)(unitOfWork ??= ServiceProviderHelper.GetService<TUoW>()); }
        }

        ILoggingService logger = null;

        /// <summary>
        /// Gets instance of log
        /// </summary>
        /// <returns>ILoggingService</returns>
        protected virtual ILoggingService Logging
        {
            get { return logger ??= LoggingService.GetLoggingService(); }
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

        protected virtual Task<bool> Validate(TEntity entity)
        {
            try
            {
                bool isValidationModel = entity.GetType()?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false;
                isValidationModel = isValidationModel || (entity.GetType()?.BaseType?.GetInterfaces()?.Any(x => x == typeof(IValidationModel<TEntity>)) ?? false);

                if (isValidationModel)
                {
                    var validator = ServiceProviderHelper.GetService<IValidatorNotify<TEntity>>();
                    if (!((IValidationModel<TEntity>)entity).IsValid(validator))
                        return TaskResult(false);
                }

                return TaskResult(true);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        #endregion
    }
}
