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
using Mvp24Hours.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork"/>
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region [ Ctor ]
        public UnitOfWork(DbContext _dbContext, INotificationContext _notificationContext, Dictionary<Type, object> _repositories)
        {
            this.DbContext = _dbContext;
            this.repositories = _repositories;
            this.NotificationContext = _notificationContext;
        }

        [ActivatorUtilitiesConstructor]
        public UnitOfWork(DbContext _dbContext, INotificationContext _notificationContext, IServiceProvider _serviceProvider)
        {
            this.DbContext = _dbContext;
            repositories = new Dictionary<Type, object>();
            this.NotificationContext = _notificationContext;
            this.serviceProvider = _serviceProvider;
        }

        #endregion

        #region [ Properties ]

        protected DbContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }
        private readonly IServiceProvider serviceProvider;

        readonly Dictionary<Type, object> repositories;

        public IRepository<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                this.repositories.Add(typeof(T), serviceProvider.GetService<IRepository<T>>());
            }
            return repositories[typeof(T)] as IRepository<T>;
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
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.SaveChanges()"/>
        /// </summary>
        public int SaveChanges(CancellationToken cancellationToken = default)
        {
            if ((NotificationContext == null || !NotificationContext.HasErrorNotifications) && !cancellationToken.IsCancellationRequested)
            {
                return this.DbContext.SaveChanges();
            }
            Rollback();
            return default;
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.Rollback()"/>
        /// </summary>
        public void Rollback()
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

        #endregion
    }
}
