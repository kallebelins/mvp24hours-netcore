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
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data projection.
    /// </summary>
    public interface IQueryMapperServiceAsync<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all representations of the typed entity.
        /// </summary>
        /// <returns>List of entities</returns>
        Task<IList<TMapper>> MapperListAsync<TMapper>() where TMapper : class;
        /// <summary>
        /// Gets all representations of the entity typed with criteria.
        /// </summary>
        /// <returns>List of entities</returns>
        Task<IList<TMapper>> MapperListAsync<TMapper>(IPagingCriteria criteria) where TMapper : class;
        /// <summary>
        /// Gets the representations based on the filter of the typed entity.
        /// </summary>
        /// <param name="clause">Filter</param>
        /// <returns>Number of representations</returns>
        Task<IList<TMapper>> MapperGetByAsync<TMapper>(Expression<Func<TEntity, bool>> clause) where TMapper : class;
        /// <summary>
        /// Gets the filter-based representations of the entity typed with criteria.
        /// </summary>
        /// <param name="clause">Filter</param>
        /// <returns>Number of representations</returns>
        Task<IList<TMapper>> MapperGetByAsync<TMapper>(Expression<Func<TEntity, bool>> clause, IPagingCriteria criteria) where TMapper : class;
        /// <summary>
        /// Gets a representation of the typed entity.
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>A representation of the entity</returns>
        Task<TMapper> MapperGetByIdAsync<TMapper>(object id) where TMapper : class;
        /// <summary>
        /// Gets a representation of the entity typed with criteria.
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>A representation of the entity</returns>
        Task<TMapper> MapperGetByIdAsync<TMapper>(object id, IPagingCriteria criteria) where TMapper : class;
    }
}
