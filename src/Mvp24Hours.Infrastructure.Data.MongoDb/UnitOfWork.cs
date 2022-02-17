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

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork"/>
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region [ Ctor ]

        public UnitOfWork(Mvp24HoursContext dbContext, INotificationContext notificationContext, Dictionary<Type, object> _repositories)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.NotificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
            this.repositories = _repositories ?? throw new ArgumentNullException(nameof(_repositories));

            DbContext.StartSession();
        }

        [ActivatorUtilitiesConstructor]
        public UnitOfWork(Mvp24HoursContext _dbContext, INotificationContext _notificationContext, IServiceProvider _serviceProvider)
        {
            this.DbContext = _dbContext ?? throw new ArgumentNullException(nameof(_dbContext));
            this.NotificationContext = _notificationContext ?? throw new ArgumentNullException(nameof(_notificationContext));
            this.serviceProvider = _serviceProvider ?? throw new ArgumentNullException(nameof(_serviceProvider));
            this.repositories = new Dictionary<Type, object>();

            DbContext.StartSession();
        }

        #endregion

        #region [ Properties ]

        private readonly Dictionary<Type, object> repositories;

        protected Mvp24HoursContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork"/>
        /// </summary>
        public IRepository<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                this.repositories.Add(typeof(T), serviceProvider.GetService<IRepository<T>>());
            }
            return repositories[typeof(T)] as IRepository<T>;
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
            if (disposing
                && this.DbContext != null)
            {
                this.DbContext = null;
            }
        }

        #endregion

        #region [ Unit of Work ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.SaveChanges()"/>
        /// </summary>
        public int SaveChanges(CancellationToken cancellationToken = default)
        {
            if (NotificationContext == null || !NotificationContext.HasErrorNotifications)
            {
                DbContext.SaveChanges(cancellationToken);
                return 1;
            }
            Rollback();
            return 0;
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork.Rollback()"/>
        /// </summary>
        public void Rollback()
        {
            DbContext.Rollback();
        }

        #endregion
    }
}
