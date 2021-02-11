//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryPagingServiceAsync<T> where T : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.ListAsync()"/>
        /// </summary>
        Task<IPagingResult<T>> PagingListAsync();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.ListAsync(IPagingCriteria)"/>
        /// </summary>
        Task<IPagingResult<T>> PagingListAsync(IPagingCriteria clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.GetByAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        Task<IPagingResult<T>> PagingGetByAsync(Expression<Func<T, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        Task<IPagingResult<T>> PagingGetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria criteria);
    }
}
