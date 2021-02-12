//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data manipulation.
    /// </summary>
    public interface ICommandService<T>
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.Add(T)"/>
        /// </summary>
        int Add(T entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.Modify(T)"/>
        /// </summary>
        int Modify(T entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{T}.Remove(T)"/>
        /// </summary>
        int Remove(T entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.ICommand{T}.RemoveById(object)"/>
        /// </summary>
        int RemoveById(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.SaveChanges()"/>
        /// </summary>
        int SaveChanges();
    }
}
