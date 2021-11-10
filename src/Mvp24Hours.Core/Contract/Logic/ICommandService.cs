//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================

using System.Collections.Generic;

namespace Mvp24Hours.Core.Contract.Logic
{
    /// <summary>
    /// Standard contract with methods for data manipulation.
    /// </summary>
    public interface ICommandService<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        void Add(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        void Add(IList<TEntity> entities);
        /// <summary>
        /// 
        /// </summary>
        void Modify(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        void Modify(IList<TEntity> entities);
        /// <summary>
        /// 
        /// </summary>
        void Remove(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        void Remove(IList<TEntity> entities);
        /// <summary>
        /// 
        /// </summary>
        void RemoveById(object id);
        /// <summary>
        /// 
        /// </summary>
        void RemoveById(IList<object> ids);
        /// <summary>
        /// <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.SaveChanges()"/>
        /// </summary>
        int SaveChanges();
    }
}
