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

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base service for using repository with paginated results and unit of work
    /// </summary>
    /// <typeparam name="TEntity">Represents an entity</typeparam>
    public class RepositoryPagingService<TEntity, TUoW> : RepositoryService<TEntity, TUoW>, IQueryService<TEntity>, ICommandService<TEntity>, IQueryPagingService<TEntity>
        where TEntity : class, IEntityBase
        where TUoW : IUnitOfWork
    {
        #region [ Implements IPagingBaseBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        public IPagingResult<IList<TEntity>> GetByWithPagination(Expression<Func<TEntity, bool>> clause)
        {
            return GetByWithPagination(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        public virtual IPagingResult<IList<TEntity>> GetByWithPagination(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria)
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

                var repo = UnitOfWork.GetRepository<TEntity>();

                var totalCount = repo.GetByCount(clause);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.GetBy(clause, criteria);

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
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List()"/>
        /// </summary>
        public IPagingResult<IList<TEntity>> ListWithPagination()
        {
            return this.ListWithPagination(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria)"/>
        /// </summary>
        public virtual IPagingResult<IList<TEntity>> ListWithPagination(IPagingCriteria criteria)
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

                var repo = UnitOfWork.GetRepository<TEntity>();

                var totalCount = repo.ListCount();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.List(criteria);

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
