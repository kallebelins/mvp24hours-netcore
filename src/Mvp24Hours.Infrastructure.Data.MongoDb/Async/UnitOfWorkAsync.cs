//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Helpers;
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

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync"/>
        /// </summary>
        public IRepositoryAsync<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), ServiceProviderHelper.GetService<IRepositoryAsync<T>>());
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
