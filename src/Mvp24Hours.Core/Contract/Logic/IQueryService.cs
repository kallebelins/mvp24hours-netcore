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

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryService<TEntity> where TEntity : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.ListAny()"/>
        /// </summary>
        bool ListAny();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.ListCount()"/>
        /// </summary>
        int ListCount();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List()"/>
        /// </summary>
        IList<TEntity> List();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List(IPagingCriteria)"/>
        /// </summary>
        IList<TEntity> List(IPagingCriteria criteria);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetByCount(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        int GetByCount(Expression<Func<TEntity, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}}, IPagingCriteria)"/>
        /// </summary>
        IList<TEntity> GetBy(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetById(int)"/>
        /// </summary>
        TEntity GetById(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        TEntity GetById(object id, IPagingCriteria criteria);
    }
}
