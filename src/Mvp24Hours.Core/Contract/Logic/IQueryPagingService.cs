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
    public interface IQueryPagingService<TEntity> where TEntity : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List()"/>
        /// </summary>
        IPagingResult<TEntity> PagingList();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List(IPagingCriteria)"/>
        /// </summary>
        IPagingResult<TEntity> PagingList(IPagingCriteria clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        IPagingResult<TEntity> PagingGetBy(Expression<Func<TEntity, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}}, IPagingCriteria)"/>
        /// </summary>
        IPagingResult<TEntity> PagingGetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria);
    }
}
