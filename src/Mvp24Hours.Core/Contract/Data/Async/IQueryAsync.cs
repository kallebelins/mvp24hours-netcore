//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Data
{
    /// <summary>
    /// Class with asynchronous functions to perform database queries (filters, sorting and pagination) for an entity
    /// </summary>
    /// <typeparam name="T">Represents an entity</typeparam>
    public interface IQueryAsync<T>
        where T : IEntityBase
    {
        /// <summary>
        /// Checks whether any records returned by the ListAsync() method
        /// </summary>
        /// <returns>Number of representations async</returns>
        Task<bool> ListAnyAsync();
        /// <summary>
        /// Gets the amount of representations returned by the ListAsync() method.
        /// </summary>
        /// <returns>Number of representations async</returns>
        Task<int> ListCountAsync();
        /// <summary>
        /// Gets all representations of the typed entity.
        /// </summary>
        /// <returns>List of entities async</returns>
        Task<IList<T>> ListAsync();
        /// <summary>
        /// Gets all representations of the entity typed with criteria.
        /// </summary>
        /// <returns>List of entities async</returns>
        Task<IList<T>> ListAsync(IPagingCriteria clause);
        /// <summary>
        /// Checks whether any records returned by the GetBy() method.
        /// </summary>
        /// <returns>Indicates whether there is a record</returns>
        Task<bool> GetByAnyAsync(Expression<Func<T, bool>> clause);
        /// <summary>
        /// Gets the amount of representations returned by the GetByAsync() method.
        /// </summary>
        /// <returns>Number of representations async</returns>
        Task<int> GetByCountAsync(Expression<Func<T, bool>> clause);
        /// <summary>
        /// Gets the representations based on the filter of the typed entity.
        /// </summary>
        /// <param name="clause">Filter</param>
        /// <returns>Number of representations async</returns>
        Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause);
        /// <summary>
        /// Gets the filter-based representations of the entity typed with criteria.
        /// </summary>
        /// <param name="clause">Filter</param>
        /// <returns>Number of representations async</returns>
        Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria criteria);
        /// <summary>
        /// Gets a representation of the typed entity.
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>A representation of the entity async</returns>
        Task<T> GetByIdAsync(object id);
        /// <summary>
        /// Gets a representation of the entity typed with criteria.
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        /// <returns>A representation of the entity async</returns>
        Task<T> GetByIdAsync(object id, IPagingCriteria clause);
    }
}
