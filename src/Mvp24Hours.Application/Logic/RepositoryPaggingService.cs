//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Application.Factory;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Logic;
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Core.DTO.Logic;
using System;
using System.Linq.Expressions;

namespace Mvp24Hours.Business.Logic
{
    /// <summary>
    /// Base business class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryPagingService<T, U> : RepositoryService<T, U>, IQueryService<T>, ICommandService<T>, IQueryPagingService<T>
        where T : class, IEntityBase
        where U : IUnitOfWork
    {
        #region [ Implements IPagingBaseBO ]

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        public IPagingResult<T> PagingGetBy(Expression<Func<T, bool>> clause)
        {
            return PagingGetBy(clause, null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria{T})"/>
        /// </summary>
        public virtual IPagingResult<T> PagingGetBy(Expression<Func<T, bool>> clause, IPagingCriteria<T> criteria)
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

                var repo = UnitOfWork.GetRepository<T>();

                var totalCount = repo.GetByCount(clause);
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.GetBy(clause, criteria);

                var result = PagingResultFactory<T>.Create(items,
                    new PageResult()
                    {
                        Count = items.Count,
                        Offset = offset,
                        Limit = limit
                    }, new SummaryResult()
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    });

                return result;
            }
            catch (Exception ex)
            {
                Logging.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List()"/>
        /// </summary>
        public IPagingResult<T> PagingList()
        {
            return this.PagingList(null);
        }

        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Logic.IQueryService{T}.List(IPagingCriteria{T})"/>
        /// </summary>
        public virtual IPagingResult<T> PagingList(IPagingCriteria<T> criteria)
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

                var repo = UnitOfWork.GetRepository<T>();

                var totalCount = repo.ListCount();
                var totalPages = (int)Math.Ceiling((double)totalCount / limit);

                var items = repo.List(criteria);

                var result = PagingResultFactory<T>.Create(items,
                    new PageResult()
                    {
                        Count = items.Count,
                        Offset = offset,
                        Limit = limit
                    }, new SummaryResult()
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    });

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
