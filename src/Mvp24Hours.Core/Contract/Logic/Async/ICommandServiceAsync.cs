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
    public interface ICommandServiceAsync<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        Task AddAsync(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        Task AddAsync(IList<TEntity> entities);
        /// <summary>
        /// 
        /// </summary>
        Task ModifyAsync(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        Task ModifyAsync(IList<TEntity> entities);
        /// <summary>
        /// 
        /// </summary>
        Task RemoveAsync(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        Task RemoveAsync(IList<TEntity> entities);
        /// <summary>
        /// 
        /// </summary>
        Task RemoveByIdAsync(object id);
        /// <summary>
        /// 
        /// </summary>
        Task RemoveByIdAsync(IList<object> ids);
        /// <summary>
        /// 
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
