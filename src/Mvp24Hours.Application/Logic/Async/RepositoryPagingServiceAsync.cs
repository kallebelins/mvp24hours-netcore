//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
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

        public Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default)
        {
            return GetByWithPaginationAsync(clause, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual async Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                int limit = MaxQtyByQueryPage;
                int offset = 0;

                if (criteria != null)
                {
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                    offset = criteria.Offset;
                }

                var repo = UnitOfWork.GetRepositoryAsync<TEntity>();

                var totalCount = await repo.GetByCountAsync(clause, cancellationToken: cancellationToken);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = await repo.GetByAsync(clause, criteria, cancellationToken: cancellationToken);

                var result = items.ToBusinessPaging(
                    new PageResult(limit, offset, items.Count),
                    new SummaryResult(totalCount, totalPages)
                );

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync()"/>
        /// </summary>
        public Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(CancellationToken cancellationToken = default)
        {
            return this.ListWithPaginationAsync(null, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public virtual async Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            try
            {
                int limit = MaxQtyByQueryPage;
                int offset = 0;

                if (criteria != null)
                {
                    limit = criteria.Limit > 0 ? criteria.Limit : limit;
                    offset = criteria.Offset;
                }

                var repo = UnitOfWork.GetRepositoryAsync<TEntity>();

                var totalCount = await repo.ListCountAsync(cancellationToken: cancellationToken);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = await repo.ListAsync(criteria, cancellationToken: cancellationToken);

                var result = items.ToBusinessPaging(
                    new PageResult(limit, offset, items.Count),
                    new SummaryResult(totalCount, totalPages)
                );

                return result;
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
