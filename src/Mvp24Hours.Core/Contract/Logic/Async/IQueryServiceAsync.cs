//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryServiceAsync<T> where T : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.ListAnyAsync()"/>
        /// </summary>
        Task<bool> ListAnyAsync();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.ListCountAsync()"/>
        /// </summary>
        Task<int> ListCountAsync();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.ListAsync()"/>
        /// </summary>
        Task<IList<T>> ListAsync();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.ListAsync(IPagingCriteria{T})"/>
        /// </summary>
        Task<IList<T>> ListAsync(IPagingCriteria<T> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetByCountAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        Task<int> GetByCountAsync(Expression<Func<T, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetByAsync(Expression{Func{T, bool}})"/>
        /// </summary>
        Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria{T})"/>
        /// </summary>
        Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria<T> criteria);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetByIdAsync(int)"/>
        /// </summary>
        Task<T> GetByIdAsync(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetByIdAsync(int, IPagingCriteria{T})"/>
        /// </summary>
        Task<T> GetByIdAsync(object id, IPagingCriteria<T> clause);
    }
}
