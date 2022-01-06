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
using Mvp24Hours.Infrastructure.Data.EFCore.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync, ISQLAsync, IDisposable
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

        #region [ ISQL Dapper ]

        public Task<IEnumerable<T>> QueryAsync<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QueryAsync<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public Task<T> QueryFirstAsync<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirstAsync<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public Task<T> QuerySingleAsync<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingleAsync<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingleOrDefaultAsync<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().ExecuteScalarAsync<T>(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        public Task<int> ExecuteAsync(string sqlQuery, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return this.DbContext.Database.GetDbConnection().ExecuteAsync(
                sqlQuery
                , param: param
                , transaction: (this.DbContext.Database?.CurrentTransaction as IInfrastructure<DbTransaction>)?.Instance
                , commandTimeout: commandTimeout ?? this.DbContext.Database.GetCommandTimeout()
                , commandType: commandType);
        }

        #endregion

        #region [ Dapper Command Definition ]

        /// <summary>
        /// Execute a query asynchronously using Task.
        /// </summary>
        public Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QueryAsync<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public Task<T> QueryFirstAsync<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirstAsync<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public Task<T> QueryFirstOrDefaultAsync<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public Task<T> QuerySingleAsync<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingleAsync<T>(command);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        public Task<T> QuerySingleOrDefaultAsync<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().QuerySingleOrDefaultAsync<T>(command);
        }

        /// <summary>
        /// Execute parameterized SQL that selects a single value.
        /// </summary>
        public Task<T> ExecuteScalarAsync<T>(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().ExecuteScalarAsync<T>(command);
        }

        /// <summary>
        /// Execute a command asynchronously using Task.
        /// </summary>
        public Task<int> ExecuteAsync(CommandDefinition command)
        {
            return this.DbContext.Database.GetDbConnection().ExecuteAsync(command);
        }

        #endregion
    }
}
