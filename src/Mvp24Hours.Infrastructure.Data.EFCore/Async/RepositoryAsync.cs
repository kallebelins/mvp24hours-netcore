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
using Mvp24Hours.Core.Contract.ValueObjects.Logic;
using Mvp24Hours.Infrastructure.Extensions;
using Mvp24Hours.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.Data.EFCore
{
    /// <summary>
    ///  <see cref="Mvp24Hours.Core.Contract.Data.IRepositoryAsync{T}"/>
    /// </summary>
    public class RepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T>, IQueryRelationAsync<T>
        where T : class, IEntityBase
    {
        #region [ Ctor ]

        public RepositoryAsync(DbContext _dbContext)
            : base(_dbContext)
        {
        }

        #endregion

        #region [ IQueryAsync ]

        public async Task<bool> ListAnyAsync()
        {
            using var scope = CreateTransactionScope(true);
            var result = await GetQuery(null, true).AnyAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        public async Task<int> ListCountAsync()
        {
            using var scope = CreateTransactionScope(true);
            var result = await GetQuery(null, true).CountAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        public Task<IList<T>> ListAsync()
        {
            return ListAsync(null);
        }

        public async Task<IList<T>> ListAsync(IPagingCriteria clause)
        {
            using var scope = CreateTransactionScope();
            var result = await GetQuery(clause).ToListAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        public async Task<bool> GetByAnyAsync(Expression<Func<T, bool>> clause)
        {
            using var scope = CreateTransactionScope(true);
            var query = dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }

            var result = await GetQuery(query, null, true).AnyAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        public async Task<int> GetByCountAsync(Expression<Func<T, bool>> clause)
        {
            using var scope = CreateTransactionScope(true);
            var query = dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }

            var result = await GetQuery(query, null, true).CountAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        public Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause)
        {
            return GetByAsync(clause, null);
        }

        public async Task<IList<T>> GetByAsync(Expression<Func<T, bool>> clause, IPagingCriteria criteria)
        {
            using var scope = CreateTransactionScope();
            var query = dbEntities.AsQueryable();
            if (clause != null)
            {
                query = query.Where(clause);
            }

            var result = await GetQuery(query, criteria).ToListAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        public Task<T> GetByIdAsync(object id)
        {
            return GetByIdAsync(id, null);
        }

        public Task<T> GetByIdAsync(object id, IPagingCriteria clause)
        {
            using var scope = CreateTransactionScope();
            var result = GetDynamicFilter(GetQuery(clause, true), GetKeyInfo(), id).SingleOrDefaultAsync();
            if (scope != null)
            {
                scope.Complete();
            }

            return result;
        }

        #endregion

        #region [ IQueryRelationAsync ]

        public Task LoadRelationAsync<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression)
            where TProperty : class
        {
            return dbContext.Entry(entity).Reference(propertyExpression).LoadAsync();
        }

        public Task LoadRelationAsync<TProperty, TKey>(T entity,
            Expression<Func<T, IEnumerable<TProperty>>> propertyExpression,
            Expression<Func<TProperty, bool>> clause = null,
            Expression<Func<TProperty, TKey>> orderKey = null,
            Expression<Func<TProperty, TKey>> orderDescendingKey = null,
            int limit = 0)
            where TProperty : class
        {
            var query = dbContext.Entry(entity).Collection(propertyExpression).Query();

            if (clause != null)
            {
                query = query.Where(clause);
            }

            if (orderKey != null)
            {
                query = query.OrderBy(orderKey);
            }

            if (orderDescendingKey != null)
            {
                query = query.OrderByDescending(orderDescendingKey);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return query.ToListAsync();
        }

        #endregion

        #region [ ICommandAsync ]

        public async Task AddAsync(T entity)
        {
            if (entity == null)
                return;

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                await dbEntities.AddAsync(entity);
            }
        }

        public Task AddAsync(IList<T> entities)
        {
            if (!entities.AnyOrNotNull())
                return Task.FromResult(false);

            return Task.WhenAll(entities?.Select(x => AddAsync(x)));
        }

        public async Task ModifyAsync(T entity)
        {
            if (entity == null)
                return;

            var entityDb = await dbContext.Set<T>().FindAsync(entity.EntityKey);

            if (entityDb == null)
                return;

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

            dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
        }

        public Task ModifyAsync(IList<T> entities)
        {
            if (!entities.AnyOrNotNull())
                return Task.FromResult(false);

            return Task.WhenAll(entities?.Select(x => ModifyAsync(x)));
        }

        public async Task RemoveAsync(T entity)
        {
            if (entity == null)
                return;

            if (entity.GetType() == typeof(IEntityLog<>))
            {
                var entityLog = entity as IEntityLog<object>;
                entityLog.Removed = TimeZoneHelper.GetTimeZoneNow();
                entityLog.RemovedBy = EntityLogBy;
                await ModifyAsync(entity);
            }
            else
            {
                await ForceRemoveAsync(entity);
            }
        }

        public Task RemoveAsync(IList<T> entities)
        {
            if (!entities.AnyOrNotNull())
                return Task.FromResult(false);

            return Task.WhenAll(entities?.Select(x => RemoveAsync(x)));
        }

        public async Task RemoveByIdAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return;
            await RemoveAsync(entity);
        }

        public Task RemoveByIdAsync(IList<object> ids)
        {
            if (!ids.AnyOrNotNull())
                return Task.FromResult(false);

            return Task.WhenAll(ids?.Select(x => RemoveByIdAsync(x)));
        }

        public Task ForceRemoveAsync(T entity)
        {
            if (entity == null)
                return Task.FromResult(false);

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                dbEntities.Attach(entity);
                dbEntities.Remove(entity);
            }
            return Task.FromResult(true);
        }

        #endregion

        #region [ Properties ]

        protected override object EntityLogBy => null;

        #endregion
    }
}
