//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
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
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.AddAsync(TEntity)"/>
        /// </summary>
        Task<int> AddAsync(TEntity entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.ModifyAsync(TEntity)"/>
        /// </summary>
        Task<int> ModifyAsync(TEntity entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{TEntity}.RemoveAsync(TEntity)"/>
        /// </summary>
        Task<int> RemoveAsync(TEntity entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync{TEntity}.RemoveByIdAsync(object)"/>
        /// </summary>
        Task<int> RemoveByIdAsync(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync.SaveChangesAsync()"/>
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
