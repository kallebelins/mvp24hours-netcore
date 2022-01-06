//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.Infrastructure.Contexts;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork"/>
    /// </summary>
    public class UnitOfWork : IUnitOfWork, ISQL, IDisposable
    {
        #region [ Ctor ]

        public UnitOfWork()
        {
            DbContext = ServiceProviderHelper.GetService<DbContext>();
            repositories = new Dictionary<Type, object>();
            NotificationContext = ServiceProviderHelper.GetService<INotificationContext>();
        }

        #endregion

        #region [ Properties ]

        protected DbContext DbContext { get; private set; }
        protected INotificationContext NotificationContext { get; private set; }

        readonly Dictionary<Type, object> repositories;

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IUnitOfWork"/>
        /// </summary>
        public IRepository<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                this.repositories.Add(typeof(T), ServiceProviderHelper.GetService<IRepository<T>>());
            }
            return repositories[typeof(T)] as IRepository<T>;
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

        #region [ ISQL ]

        public IEnumerable<T> Query<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().Query<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public T QueryFirst<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirst<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public T QueryFirstOrDefault<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirstOrDefault<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public T QuerySingle<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingle<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public T QuerySingleOrDefault<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingleOrDefault<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public T ExecuteScalar<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().ExecuteScalar<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public int Execute(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().Execute(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        #endregion

        #region [ Dapper Command Definition ]

        /// <summary>
        /// Execute a query hronously using Task.
        /// </summary>
        public IEnumerable<T> Query<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().Query<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public T QueryFirst<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirst<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public T QueryFirstOrDefault<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirstOrDefault<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public T QuerySingle<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingle<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public T QuerySingleOrDefault<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingleOrDefault<T>(command);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        public T ExecuteScalar<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().ExecuteScalar<T>(command);
        }

        /// <summary>
        /// Execute a command hronously using Task.
        /// </summary>
        public int Execute(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().Execute(command);
        }

        #endregion
    }
}
