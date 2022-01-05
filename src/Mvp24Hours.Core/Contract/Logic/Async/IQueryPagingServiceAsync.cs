//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryPagingServiceAsync<TEntity> where TEntity : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.ListAsync(CancellationToken)"/>
        /// </summary>
        Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.ListAsync(IPagingCriteria, CancellationToken)"/>
        /// </summary>
        Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(IPagingCriteria criteria, CancellationToken cancellationToken = default);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}}, CancellationToken)"/>
        /// </summary>
        Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, CancellationToken cancellationToken = default);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}}, IPagingCriteria, CancellationToken)"/>
        /// </summary>
        Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria, CancellationToken cancellationToken = default);
    }
}
