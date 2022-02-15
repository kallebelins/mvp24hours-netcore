//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.DependencyInjection;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    /// <summary>
    ///  <see cref="IUnitOfWorkAsync"/>
    /// </summary>
    public class UnitOfWorkAsync : IUnitOfWorkAsync, IDisposable
    {
        #region [ Ctor ]

        public UnitOfWorkAsync(Mvp24HoursContext dbContext, INotificationContext notificationContext, Dictionary<Type, object> _repositories)
        {
            this.DbContext = dbContext;
            this.repositories = _repositories;
            this.NotificationContext = notificationContext;

            DbContext.StartSessionAsync().Wait();
        }

        [ActivatorUtilitiesConstructor]
        public UnitOfWorkAsync(Mvp24HoursContext _dbContext, INotificationContext _notificationContext, IServiceProvider _serviceProvider)
        {
            this.DbContext = _dbContext;
            repositories = new Dictionary<Type, object>();
            this.NotificationContext = _notificationContext;
            this.serviceProvider = _serviceProvider;

            DbContext.StartSessionAsync().Wait();
        }

        #endregion

        #region [ Ctor ]

        public UnitOfWorkAsync(Mvp24HoursContext dbContext, INotificationContext notificationContext)
        {
            this.DbContext = dbContext;
            repositories = new Dictionary<Type, object>();
            this.NotificationContext = notificationContext;

            DbContext.StartSessionAsync().Wait();
        }

        #endregion

        #region [ Properties ]

        private readonly Dictionary<Type, object> repositories;

        protected Mvp24HoursContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync"/>
        /// </summary>
        public IRepositoryAsync<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), serviceProvider.GetService<IRepositoryAsync<T>>());
            }
            return repositories[typeof(T)] as IRepositoryAsync<T>;
        }

        [Obsolete("MongoDb does not support IDbConnection. Use the database (IMongoDatabase) from context.")]
        public IDbConnection GetConnection()
        {
            throw new NotSupportedException("MongoDb does not support IDbConnection. Use the database (IMongoDatabase) from context.");
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
                if (DbContext != null)
                {
                    DbContext = null;
                }
            }
        }

        #endregion

        #region [ Unit of Work ]

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync.SaveChangesAsync(CancellationToken)"/>
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (NotificationContext == null || !NotificationContext.HasErrorNotifications)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
                return 1;
            }
            await RollbackAsync();
            return 0;
        }

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync.RollbackAsync()"/>
        /// </summary>
        public async Task RollbackAsync()
        {
            await DbContext.RollbackAsync();
        }

        #endregion
    }
}
