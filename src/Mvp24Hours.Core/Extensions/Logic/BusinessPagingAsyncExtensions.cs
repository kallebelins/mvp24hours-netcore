//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Core.DTOs.Models;
using Mvp24Hours.Core.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class BusinessPagingAsyncExtensions
    {
        public static async Task<IPagingCriteria> ToPagingCriteriaAsync(this Task<PagingCriteriaRequest> requestAsync)
        {
            var request = await requestAsync;
            return new PagingCriteria(
                request.Limit,
                request.Offset,
                new ReadOnlyCollection<string>(request.OrderBy ?? new List<string>()),
                new ReadOnlyCollection<string>(request.Navigation ?? new List<string>())
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<IPagingResult<T>> ToBusinessPagingAsync<T>(this Task<T> valueAsync, IPageResult page, ISummaryResult summary, IList<IMessageResult> messageResult = null, string tokenDefault = null)
        {
            T value = default;
            if (valueAsync != null)
            {
                value = await valueAsync;
            }
            return new PagingResult<T>(
                page,
                summary,
                value,
                messages: new ReadOnlyCollection<IMessageResult>(messageResult ?? new List<IMessageResult>()),
                token: tokenDefault
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<IPagingResult<T>> ToBusinessPagingAsync<T>(this Task<T> valueAsync, IList<IMessageResult> messageResult, string tokenDefault = null)
        {
            T value = default;
            if (valueAsync != null)
            {
                value = await valueAsync;
            }
            return new PagingResult<T>(
                new PageResult(0, 0, 0),
                new SummaryResult(0, 0),
                data: value,
                messages: new ReadOnlyCollection<IMessageResult>(messageResult ?? new List<IMessageResult>()),
                token: tokenDefault
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<IPagingResult<IList<TEntity>>> ToBusinessPagingAsync<TEntity>(this IRepositoryAsync<TEntity> repository, Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria = null, int maxQtyByQueryDefault = 300)
            where TEntity : class, IEntityBase
        {
            int limit = maxQtyByQueryDefault;
            int offset = 0;

            if (criteria != null)
            {
                limit = criteria.Limit > 0 ? criteria.Limit : limit;
                offset = criteria.Offset;
            }

            var totalCount = await repository.GetByCountAsync(clause);
            var totalPages = (int)Math.Ceiling((double)totalCount / limit);

            var items = await repository.GetByAsync(clause, criteria);

            var result = items.ToBusinessPaging(
                new PageResult(limit, offset, items.Count),
                new SummaryResult(totalCount, totalPages)
            );

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task<IPagingResult<IList<TEntity>>> ToBusinessPagingAsync<TEntity>(this IRepositoryAsync<TEntity> repository, IPagingCriteria criteria = null, int maxQtyByQueryDefault = 300)
            where TEntity : class, IEntityBase
        {
            int limit = maxQtyByQueryDefault;
            int offset = 0;

            if (criteria != null)
            {
                limit = criteria.Limit > 0 ? criteria.Limit : limit;
                offset = criteria.Offset;
            }

            var totalCount = await repository.ListCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / limit);

            var items = await repository.ListAsync(criteria);

            var result = items.ToBusinessPaging(
                new PageResult(limit, offset, items.Count),
                new SummaryResult(totalCount, totalPages)
            );

            return result;
        }
    }
}
