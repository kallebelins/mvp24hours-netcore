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
    /// Design Pattern: Repository
    /// Description: Mediation between domain and data mapping layers using a collection as 
    /// an interface for accessing domain objects. (Martin Fowler)
    /// Learn more: http://martinfowler.com/eaaCatalog/repository.html
    /// </summary>
    /// <typeparam name="T">IEntity || IEntityBaseGuid</typeparam>
    public interface ICommand<T>
        where T : IEntityBase
    {
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="entity">Entity instance</param>
        void Add(T entity);
        /// <summary>
        /// Adds list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        void Add(IList<T> entities);
        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entity instance</param>
        void Modify(T entity);
        /// <summary>
        /// Updates list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        void Modify(IList<T> entities);
        /// <summary>
        /// Removes an entity (logical exclusion).
        /// </summary>
        /// <param name="entity">Entity instance</param>
        void Remove(T entity);
        /// <summary>
        /// Removes list of entities (logical exclusion).
        /// </summary>
        /// <param name="entities">List of entities</param>
        void Remove(IList<T> entities);
        /// <summary>
        /// Removes an entity by the code identifier (logical exclusion).
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        void Remove(object id);
    }
}
