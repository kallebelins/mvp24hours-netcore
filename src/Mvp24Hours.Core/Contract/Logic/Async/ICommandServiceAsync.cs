//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data manipulation.
    /// </summary>
    public interface ICommandServiceAsync<T>
        where T : class
    {
        /// <summary>
        /// Add an entities.
        /// </summary>
        /// <param name="entity"></param>
        Task<int> AddAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Adds list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        Task<int> AddAsync(IList<T> entities, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entity instance</param>
        Task<int> ModifyAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        Task<int> ModifyAsync(IList<T> entities, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes an entity (logical exclusion).
        /// </summary>
        /// <param name="entity">Entity instance</param>
        Task<int> RemoveAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes list of entities (logical exclusion).
        /// </summary>
        /// <param name="entities">List of entities</param>
        Task<int> RemoveAsync(IList<T> entities, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes an entity by the code identifier (logical exclusion).
        /// </summary>
        /// <param name="id">Identifier of entity</param>
        Task<int> RemoveByIdAsync(object id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes an entity by the code identifier (logical exclusion).
        /// </summary>
        /// <param name="id">List of identifiers</param>
        Task<int> RemoveByIdAsync(IList<object> ids, CancellationToken cancellationToken = default);
    }
}
