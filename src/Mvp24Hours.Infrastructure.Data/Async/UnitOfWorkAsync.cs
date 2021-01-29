//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Infrastructure.Data.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync"/>
    /// </summary>
    public class UnitOfWorkAsync : IUnitOfWorkAsync, IDisposable
    {
        #region [ Ctor ]

        public UnitOfWorkAsync()
        {
            this.DbContext = HttpContextHelper.GetService<DbContext>();
            this.repositories = new Dictionary<Type, object>();
        }

        #endregion

        #region [ Properties ]

        protected DbContext DbContext { get; private set; }

        Dictionary<Type, object> repositories;

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync"/>
        /// </summary>
        public IRepositoryAsync<T> GetRepositoryAsync<T>()
            where T : class, IEntityBase
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                this.repositories.Add(typeof(T), (IRepositoryAsync<T>)new RepositoryAsync<T>(this.DbContext));
            }
            return repositories[typeof(T)] as IRepositoryAsync<T>;
        }

        #endregion

        #region [ IDisposable ]

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.DbContext != null)
                {
                    this.DbContext.Dispose();
                }
            }
        }

        #endregion

        #region [ Unit of Work ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync.SaveChangesAsync(CancellationToken cancellationToken)"/>
        /// </summary>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync.RollbackAsync()"/>
        /// </summary>
        public void RollbackAsync()
        {
            var changedEntries = this.DbContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ISQLAsync.ExecuteQueryAsync(string, object[])"/>
        /// </summary>
        public Task<IList<T>> ExecuteQueryAsync<T>(string sqlQuery, params object[] parameters) where T : class
        {
            return this.DbContext.SqlQueryAsync<T>(sqlQuery, parameters);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ISQLAsync.ExecuteCommandAsync(string, object[])"/>
        /// </summary>
        public Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters)
        {
            return this.DbContext.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }

        #endregion
    }
}
