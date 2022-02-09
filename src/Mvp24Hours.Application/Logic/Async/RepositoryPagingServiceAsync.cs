//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.Logic
{
    /// <summary>
    /// Asynchronous service for using repository with paginated results and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryPagingServiceAsync<TEntity, TUoW> : RepositoryServiceAsync<TEntity, TUoW>, IQueryServiceAsync<TEntity>, ICommandServiceAsync<TEntity>, IQueryPagingServiceAsync<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWorkAsync
    {
        #region [ Implements IPagingBaseAsyncBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual async Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var repo = UnitOfWork.GetRepository<TEntity>();
                return await repo.ToBusinessPagingAsync(clause, criteria);
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public virtual async Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(IPagingCriteria criteria = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var repo = UnitOfWork.GetRepository<TEntity>();
                return await repo.ToBusinessPagingAsync(criteria);
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
