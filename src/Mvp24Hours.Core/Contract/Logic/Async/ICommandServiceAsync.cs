//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data manipulation.
    /// </summary>
    public interface ICommandServiceAsync<T>
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.AddAsync(T)"/>
        /// </summary>
        Task<int> AddAsync(T entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.ModifyAsync(T)"/>
        /// </summary>
        Task<int> ModifyAsync(T entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.RemoveAsync(T)"/>
        /// </summary>
        Task<int> RemoveAsync(T entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync{T}.RemoveAsync(int)"/>
        /// </summary>
        Task<int> RemoveAsync(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync.SaveChangesAsync()"/>
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
