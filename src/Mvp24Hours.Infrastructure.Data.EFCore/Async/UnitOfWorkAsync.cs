//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync, IDisposable
    {
        #region [ Ctor ]

        public UnitOfWorkAsync()
        {
            this.DbContext = ServiceProviderHelper.GetService<DbContext>();
            this.repositories = new Dictionary<Type, object>();
            this.NotificationContext = ServiceProviderHelper.GetService<INotificationContext>();
        }

        #endregion

        #region [ Properties ]

        protected DbContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }

        private readonly Dictionary<Type, object> repositories;

        public IRepositoryAsync<T> GetRepositoryAsync<T>()
            where T : class, IEntityBase
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                this.repositories.Add(typeof(T), ServiceProviderHelper.GetService<IRepositoryAsync<T>>());
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

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (NotificationContext == null || !NotificationContext.HasErrorNotifications || !cancellationToken.IsCancellationRequested)
            {
                return await this.DbContext.SaveChangesAsync(cancellationToken);
            }
            await RollbackAsync(cancellationToken);
            return default;
        }
        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return default;
            }

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
            return default;
        }

        #endregion
    }
}
