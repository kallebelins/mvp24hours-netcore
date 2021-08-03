//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection relationship.
    /// </summary>
    public interface IQueryRelationService<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryRelation{TEntity}.LoadRelation{TProperty}(TEntity, Expression{Func{TEntity, TProperty}})"/>
        /// </summary>
        void LoadRelation<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression)
            where TProperty : class;
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryRelation{TEntity}.LoadRelation{TProperty, TKey}(TEntity, Expression{Func{TEntity, IEnumerable{TProperty}}}, Expression{Func{TProperty, bool}}, Expression{Func{TProperty, TKey}}, Expression{Func{TProperty, TKey}}, int)"/>
        /// </summary>
        void LoadRelation<TProperty, TKey>(TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            Expression<Func<TProperty, bool>> clause = null,
            Expression<Func<TProperty, TKey>> orderKey = null,
            Expression<Func<TProperty, TKey>> orderDescendingKey = null,
            int limit = 0)
            where TProperty : class;
    }
}
