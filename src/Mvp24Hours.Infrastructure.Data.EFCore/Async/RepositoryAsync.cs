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

        public RepositoryAsync(DbContext dbContext)
            : base(dbContext)
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

        public void AddAsync(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var entry = dbContext.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                dbEntities.AddAsync(entity);
            }
        }

        public void AddAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    AddAsync(entity);
                }
            }
        }

        public void ModifyAsync(T entity)
        {
            if (entity == null)
            {
                return;
            }

            var entityDb = dbContext.Set<T>().Find(entity.EntityKey);

            if (entityDb == null)
            {
                return;
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

            dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
        }

        public void ModifyAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    ModifyAsync(entity);
                }
            }
        }

        public void RemoveAsync(T entity)
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
                ModifyAsync(entity);
            }
            else
            {
                ForceRemoveAsync(entity);
            }
        }

        public void RemoveAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    RemoveAsync(entity);
                }
            }
        }

        public void RemoveByIdAsync(object id)
        {
            var entity = GetByIdAsync(id);
            if (entity == null)
            {
                return;
            }

            RemoveByIdAsync(entity);
        }

        public void ForceRemoveAsync(T entity)
        {
            if (entity == null)
            {
                return;
            }

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
        }

        #endregion

        #region [ Properties ]

        protected override object EntityLogBy => null;

        #endregion
    }
}
