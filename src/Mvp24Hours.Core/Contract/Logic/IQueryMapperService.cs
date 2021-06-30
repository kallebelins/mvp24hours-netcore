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
    public interface IQueryMapperService<TEntity> where TEntity : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List()"/>
        /// </summary>
        IList<TMapper> MapperList<TMapper>() where TMapper : class;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.List(IPagingCriteria)"/>
        /// </summary>
        IList<TMapper> MapperList<TMapper>(IPagingCriteria criteria) where TMapper : class;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        IList<TMapper> MapperGetBy<TMapper>(Expression<Func<TEntity, bool>> clause) where TMapper : class;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetBy(Expression{Func{TEntity, bool}}, IPagingCriteria)"/>
        /// </summary>
        IList<TMapper> MapperGetBy<TMapper>(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria) where TMapper : class;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetById(int)"/>
        /// </summary>
        TMapper MapperGetById<TMapper>(object id) where TMapper : class;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.GetById(int, IPagingCriteria)"/>
        /// </summary>
        TMapper MapperGetById<TMapper>(object id, IPagingCriteria criteria) where TMapper : class;
    }
}
