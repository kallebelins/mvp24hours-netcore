//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryPagingService<TEntity>
        where TEntity : IEntityBase
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List()"/>
        /// </summary>
        IPagingResult<IList<TEntity>> ListWithPagination();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List(IPagingCriteria)"/>
        /// </summary>
        IPagingResult<IList<TEntity>> ListWithPagination(IPagingCriteria criteria);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        IPagingResult<IList<TEntity>> GetByWithPagination(Expression<Func<TEntity, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}}, IPagingCriteria)"/>
        /// </summary>
        IPagingResult<IList<TEntity>> GetByWithPagination(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria);
    }
}
