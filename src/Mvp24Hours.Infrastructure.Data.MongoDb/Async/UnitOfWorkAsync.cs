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
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
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

        public UnitOfWorkAsync()
        {
            DbContext = ServiceProviderHelper.GetService<Mvp24HoursMongoDbContext>();
            repositories = new Dictionary<Type, object>();
            NotificationContext = ServiceProviderHelper.GetService<INotificationContext>();

            DbContext.StartSessionAsync().Wait();
        }

        #endregion

        #region [ Properties ]

        private readonly Dictionary<Type, object> repositories;

        protected Mvp24HoursMongoDbContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync"/>
        /// </summary>
        public IRepositoryAsync<T> GetRepositoryAsync<T>()
            where T : class, IEntityBase
        {
            if (!repositories.ContainsKey(typeof(T)))
            {
                repositories.Add(typeof(T), ServiceProviderHelper.GetService<IRepositoryAsync<T>>());
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
                if (DbContext != null)
                {
                    DbContext = null;
                }
            }
        }

        #endregion

        #region [ Unit of Work ]

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync.SaveChangesAsync()"/>
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (NotificationContext == null || !NotificationContext.HasErrorNotifications)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
                return 1;
            }
            await RollbackAsync(cancellationToken);
            return 0;
        }

        /// <summary>
        ///  <see cref="IUnitOfWorkAsync.RollbackAsync()"/>
        /// </summary>
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await DbContext.RollbackAsync(cancellationToken);
        }

        #endregion
    }
}
