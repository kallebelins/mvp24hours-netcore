//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Data
{
    /// <summary>
    /// Class with asynchronous functions to perform database commands (add, modify or delete) for an entity
    /// </summary>
    /// <typeparam name="T">Represents an entity</typeparam>
    public interface ICommandAsync<T>
        where T : IEntityBase
    {
        /// <summary>
        /// Add an entities.
        /// </summary>
        /// <param name="entity"></param>
        void AddAsync(T entity);
        /// <summary>
        /// Adds list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        void AddAsync(IList<T> entities);
        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entity instance</param>
        void ModifyAsync(T entity);
        /// <summary>
        /// Updates list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        void ModifyAsync(IList<T> entities);
        /// <summary>
        /// Removes an entity (logical exclusion).
        /// </summary>
        /// <param name="entity">Entity instance</param>
        void RemoveAsync(T entity);
        /// <summary>
        /// Removes list of entities (logical exclusion).
        /// </summary>
        /// <param name="entities">List of entities</param>
        void RemoveAsync(IList<T> entities);
        /// <summary>
        /// Removes an entity by the code identifier (logical exclusion).
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        void RemoveByIdAsync(object id);
    }
}
