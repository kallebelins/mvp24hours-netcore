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

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryPagingService<T> where T : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.List()"/>
        /// </summary>
        IPagingResult<T> PagingList();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.List(IPagingCriteria)"/>
        /// </summary>
        IPagingResult<T> PagingList(IPagingCriteria clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        IPagingResult<T> PagingGetBy(Expression<Func<T, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        IPagingResult<T> PagingGetBy(Expression<Func<T, bool>> clause, IPagingCriteria criteria);
    }
}
