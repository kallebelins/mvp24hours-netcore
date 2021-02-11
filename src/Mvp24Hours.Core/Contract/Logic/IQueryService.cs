//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
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
    public interface IQueryService<T> where T : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.ListAny()"/>
        /// </summary>
        bool ListAny();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.ListCount()"/>
        /// </summary>
        int ListCount();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.List()"/>
        /// </summary>
        IList<T> List();
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.List(IPagingCriteria)"/>
        /// </summary>
        IList<T> List(IPagingCriteria criteria);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetByCount(Expression{Func{T, bool}})"/>
        /// </summary>
        int GetByCount(Expression<Func<T, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetBy(Expression{Func{T, bool}})"/>
        /// </summary>
        IList<T> GetBy(Expression<Func<T, bool>> clause);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetBy(Expression{Func{T, bool}}, IPagingCriteria)"/>
        /// </summary>
        IList<T> GetBy(Expression<Func<T, bool>> clause, IPagingCriteria criteria);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetById(int)"/>
        /// </summary>
        T GetById(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        T GetById(object id, IPagingCriteria criteria);
    }
}
