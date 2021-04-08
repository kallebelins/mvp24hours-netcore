//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Domain.Entity;
using System;

namespace Mvp24Hours.Core.Contract.Data
{
    /// <summary>
    /// Desing Pattern: UnitOfWork
    /// Description: Maintains a list of objects affected by a business transaction and 
	/// coordinates writing out changes and resolving competition issues. (Martin Fowler)
    /// Learn more: http://martinfowler.com/eaaCatalog/unitOfWork.html
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Persists the actions taken in the transaction
        /// </summary>
        int SaveChanges();
        /// <summary>
        /// Discard changes in transaction
        /// </summary>
        void Rollback();
        /// <summary>
        /// Gets a repository
        /// </summary>
        /// <returns>A repository for an entity instance</returns>
        IRepository<T> GetRepository<T>() where T : class, IEntityBase;
    }
}
