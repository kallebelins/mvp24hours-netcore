//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Asynchronous service for using repository with paginated results and unit of work
    /// </summary>
    /// <typeparam name="T">Represents an entity</typeparam>
    public class RepositoryPagingServiceAsync<T, U> : RepositoryServiceAsync<T, U>, IQueryServiceAsync<T>, ICommandServiceAsync<T>, IQueryPagingServiceAsync<T>
        where T : class, IEntityBase
        where U : IUnitOfWorkAsync
    {
        #region [ Implements IPagingBaseAsyncBO ]

        public Task<IPagingResult<T>> PagingGetByAsync(Expression<Func<T, bool>> clause)
        {
            return PagingGetByAsync(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual async Task<IPagingResult<T>> PagingGetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria criteria)
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

                var repo = UnitOfWork.GetRepositoryAsync<T>();

                var totalCount = await repo.GetByCountAsync(clause);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = await repo.GetByAsync(clause, criteria);

                var result = await items.ToBusinessPaggingAsync(
                    new PageResult(limit, offset, items.Count),
                    new SummaryResult(totalCount, totalPages)
                );

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync()"/>
        /// </summary>
        public Task<IPagingResult<T>> PagingListAsync()
        {
            return this.PagingListAsync(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryServiceAsync{T}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        public async virtual Task<IPagingResult<T>> PagingListAsync(IPagingCriteria criteria)
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

                var repo = UnitOfWork.GetRepositoryAsync<T>();

                var totalCount = await repo.ListCountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = await repo.ListAsync(criteria);

                var result = await items.ToBusinessPaggingAsync(
                    new PageResult(limit, offset, items.Count),
                    new SummaryResult(totalCount, totalPages)
                );

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        #endregion
    }
}
