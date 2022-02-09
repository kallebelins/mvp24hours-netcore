//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
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
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.ListAsync(IPagingCriteria, CancellationToken)"/>
        /// </summary>
        Task<IPagingResult<IList<TEntity>>> ListWithPaginationAsync(IPagingCriteria criteria = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.GetByAsync(Expression{Func{TEntity, bool}}, IPagingCriteria, CancellationToken)"/>
        /// </summary>
        Task<IPagingResult<IList<TEntity>>> GetByWithPaginationAsync(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria = null, CancellationToken cancellationToken = default);
    }
}
