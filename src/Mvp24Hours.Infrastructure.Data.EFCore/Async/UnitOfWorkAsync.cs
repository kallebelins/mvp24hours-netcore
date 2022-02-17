//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWorkAsync"/>
    /// </summary>
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        #region [ Ctor ]
        public UnitOfWorkAsync(DbContext _dbContext, INotificationContext _notificationContext, Dictionary<Type, object> _repositories)
        {
            this.DbContext = _dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
            this.NotificationContext = _notificationContext ?? throw new ArgumentNullException(nameof(_notificationContext));
            this.repositories = _repositories ?? throw new ArgumentNullException(nameof(_repositories));
        }

        [ActivatorUtilitiesConstructor]
        public UnitOfWorkAsync(DbContext _dbContext, INotificationContext _notificationContext, IServiceProvider _serviceProvider)
        {
            this.DbContext = _dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
            this.NotificationContext = _notificationContext ?? throw new ArgumentNullException(nameof(_notificationContext));
            this.serviceProvider = _serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
            this.repositories = new Dictionary<Type, object>();
        }

        #endregion

        #region [ Properties ]

        protected DbContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }

        private readonly Dictionary<Type, object> repositories;
        private readonly IServiceProvider serviceProvider;

        public IRepositoryAsync<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                this.repositories.Add(typeof(T), serviceProvider.GetService<IRepositoryAsync<T>>());
            }
            return repositories[typeof(T)] as IRepositoryAsync<T>;
        }

        public IDbConnection GetConnection()
        {
            return DbContext?.Database?.GetDbConnection();
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
            if (disposing
                && this.DbContext != null)
            {
                this.DbContext.Dispose();
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
            await RollbackAsync();
            return default;
        }
        public Task RollbackAsync()
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
            return default;
        }

        #endregion
    }
}
