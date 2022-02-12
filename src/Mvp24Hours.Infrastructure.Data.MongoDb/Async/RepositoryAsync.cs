//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Mvp24Hours.Core.Contract.Data;
using Mvp24Hours.Core.Contract.Domain.Entity;
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using Mvp24Hours.Infrastructure.Data.MongoDb.Base;
using Mvp24Hours.Infrastructure.Data.MongoDb.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.MongoDb
{
    public class RepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        public RepositoryAsync(Mvp24HoursContext dbContext, IOptions<MongoDbRepositoryOptions> options)
            : base(dbContext, options)
        {
        }

        #endregion

        #region [ IQueryAsync ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListAnyAsync(CancellationToken)"/>
        /// </summary>
        public async Task<bool> ListAnyAsync(CancellationToken cancellationToken = default)
        {
            return await GetQuery(null, true)
                .AnyAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListCountAsync(CancellationToken)"/>
        /// </summary>
        public async Task<int> ListCountAsync(CancellationToken cancellationToken = default)
        {
            return await GetQuery(null, true)
                .CountAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListAsync(CancellationToken)"/>
        /// </summary>
        public async Task<IList<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await ListAsync(null, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.ListAsync(IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public async Task<IList<T>> ListAsync(IPagingCriteria clause, CancellationToken cancellationToken = default)
        {
            return await GetQuery(clause)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByAnyAsync(Expression{Func{T, bool}}, CancellationToken)"/>
        /// </summary>
        public async Task<bool> GetByAnyAsync(Expression<Func<T, bool>> clause, CancellationToken cancellationToken = default)
        {
            var query = dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }
            return await GetQuery(query, null, true)
                .AnyAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByCountAsync(Expression{Func{T, bool}}, CancellationToken)"/>
        /// </summary>
        public async Task<int> GetByCountAsync(Expression<Func<T, bool>> clause, CancellationToken cancellationToken = default)
        {
            var query = dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }
            return await GetQuery(query, null, true)
                .CountAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByAsync(Expression{Func{T, bool}}, CancellationToken)"/>
        /// </summary>
        public async Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, CancellationToken cancellationToken = default)
        {
            return await GetByAsync(clause, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByAsync(Expression{Func{T, bool}}, IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public async Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria criteria, CancellationToken cancellationToken = default)
        {
            var query = dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }
            return await GetQuery(query, criteria)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByIdAsync(object, CancellationToken)"/>
        /// </summary>
        public async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, null, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.GetByIdAsync(object, IPagingCriteria, CancellationToken)"/>
        /// </summary>
        public async Task<T> GetByIdAsync(object id, IPagingCriteria clause, CancellationToken cancellationToken = default)
        {
            return await GetDynamicFilter(GetQuery(clause, true), GetKeyInfo(), id)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        #endregion

        #region [ ICommandAsync ]

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.IQueryAsync.AddAsync(T)"/>
        /// </summary>
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return;
            }
            await dbEntities.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.AddAsync(IList{T})"/>
        /// </summary>
        public async Task AddAsync(IList<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities.AnyOrNotNull())
            {
                foreach (var entity in entities)
                {
                    await AddAsync(entity, cancellationToken: cancellationToken);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.ModifyAsync(T)"/>
        /// </summary>
        public async Task ModifyAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return;
            }

            var entityDb = (await dbContext.Set<T>().FindAsync(GetKeyFilter(entity), cancellationToken: cancellationToken)).FirstOrDefault();

            if (entityDb == null)
            {
                throw new InvalidOperationException("Key value not found.");
            }

            // properties that can not be changed

            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                var entityDbLog = entityDb as IEntityLog<object>;
                entityLog.Created = entityDbLog.Created;
                entityLog.CreatedBy = entityDbLog.CreatedBy;
                entityLog.Modified = entityDbLog.Modified;
                entityLog.ModifiedBy = entityDbLog.ModifiedBy;
            }

            await dbEntities.ReplaceOneAsync(GetKeyFilter(entity), entity, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.ModifyAsync(List)"/>
        /// </summary>
        public async Task ModifyAsync(IList<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities.AnyOrNotNull())
            {
                foreach (var entity in entities)
                {
                    await ModifyAsync(entity, cancellationToken: cancellationToken);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(T)"/>
        /// </summary>
        public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                entityLog.Removed = TimeZoneHelper.GetTimeZoneNow();
                entityLog.RemovedBy = EntityLogBy;
                await ModifyAsync(entity, cancellationToken: cancellationToken);
            }
            else
            {
                await ForceRemoveAsync(entity, cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(List)"/>
        /// </summary>
        public async Task RemoveAsync(IList<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities.AnyOrNotNull())
            {
                foreach (var entity in entities)
                {
                    await RemoveAsync(entity, cancellationToken: cancellationToken);
                }
            }
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(int)"/>
        /// </summary>
        public async Task RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }
            await RemoveAsync(entity, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///  <see cref="Mvp24Hours.Core.Contract.Data.ICommandAsync.RemoveAsync(IList{TEntity})"/>
        /// </summary>
        public async Task RemoveByIdAsync(IList<object> ids, CancellationToken cancellationToken = default)
        {
            if (ids.AnyOrNotNull())
            {
                foreach (var id in ids)
                {
                    await RemoveByIdAsync(id, cancellationToken: cancellationToken);
                }
            }
        }

        /// <summary>
        ///  If entity is not log
        /// </summary>
        private async Task ForceRemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return;
            }
            await dbEntities.DeleteOneAsync(GetKeyFilter(entity), cancellationToken: cancellationToken);
        }

        #endregion

        #region [ IQueryRelationAsync ]
        public Task LoadRelationAsync<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            throw new NotSupportedException();
        }
        public Task LoadRelationAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Expression<Func<TProperty, bool>> clause = null, int limit = 0, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            throw new NotSupportedException();
        }
        public Task LoadRelationSortByAscendingAsync<TProperty, TKey>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Expression<Func<TProperty, TKey>> orderKey, Expression<Func<TProperty, bool>> clause = null, int limit = 0, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            throw new NotSupportedException();
        }
        public Task LoadRelationSortByDescendingAsync<TProperty, TKey>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression, Expression<Func<TProperty, TKey>> orderKey, Expression<Func<TProperty, bool>> clause = null, int limit = 0, CancellationToken cancellationToken = default)
            where TProperty : class
        {
            throw new NotSupportedException();
        }
        #endregion

        #region [ Properties ]

        protected override object EntityLogBy => throw new NotSupportedException();

        #endregion
    }
}
