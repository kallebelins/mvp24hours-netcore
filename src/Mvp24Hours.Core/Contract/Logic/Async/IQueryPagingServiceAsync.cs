//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryPagingServiceAsync<TEntity> where TEntity : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.ListAsync()"/>
        /// </summary>
        Task<IPagingResult<TEntity>> PagingListAsync();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        Task<IPagingResult<TEntity>> PagingListAsync(IPagingCriteria clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        Task<IPagingResult<TEntity>> PagingGetByAsync(Expression<Func<TEntity, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}}, IPagingCriteria)"/>
        /// </summary>
        Task<IPagingResult<TEntity>> PagingGetByAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria);
    }
}
