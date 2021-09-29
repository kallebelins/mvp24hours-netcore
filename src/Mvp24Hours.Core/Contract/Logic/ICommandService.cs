//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data manipulation.
    /// </summary>
    public interface ICommandService<TEntity>
    {
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.Add(TEntity)"/>
        /// </summary>
        int Add(TEntity entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.Modify(TEntity)"/>
        /// </summary>
        int Modify(TEntity entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IQuery{TEntity}.Remove(TEntity)"/>
        /// </summary>
        int Remove(TEntity entity);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.ICommand{TEntity}.RemoveById(object)"/>
        /// </summary>
        int RemoveById(object id);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.SaveChanges()"/>
        /// </summary>
        int SaveChanges();
    }
}
